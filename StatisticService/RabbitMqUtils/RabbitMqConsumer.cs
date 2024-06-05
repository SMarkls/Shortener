using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StatisticService.Models;
using StatisticService.Persistence;
using StatisticService.Utils;

namespace StatisticService.RabbitMqUtils;

public class RabbitMqConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ChannelManager _channelManager;

    public RabbitMqConsumer(IServiceScopeFactory scopeFactory, ChannelManager channelManager)
    {
        _scopeFactory = scopeFactory;
        _channelManager = channelManager;
    }

    public async Task MessageReceived(object? sender, BasicDeliverEventArgs args)
    {
        var statistic = await DeserializeBody<Statistic>(args.Body);
        if (statistic is null)
        {
            return;
        }

        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Statistics.AddAsync(statistic);
        await context.SaveChangesAsync();
    }

    public async Task GetStatisticsProcedureCalled(object? sender, BasicDeliverEventArgs args)
    {
        var callingChannel = await _channelManager.GetRpcCallingChannel();
        var token = await DeserializeBody<string>(args.Body);
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var list = await context.Statistics.Where(x => x.Token == token).ToListAsync();
            var countOfUnique = list.DistinctBy(x => x.IpAddress).Count();
            var vm = new StatisticVm
            {
                Statistics = list.Select(x => new CompressedStatistic
                { Browser = BrowserParser.GetBrowserName(x.Browser), Time = x.Time }).ToList(),
                CountOfUniqueUsers = countOfUnique
            };

            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, vm, typeof(StatisticVm));
            var message = stream.GetBuffer();
            var returningChannel = await _channelManager.GetRpcReturningChannel();
            await returningChannel.BasicPublishAsync(exchange: string.Empty,
                routingKey: "rpc_return_statistic",
                basicProperties: new BasicProperties
                { ReplyTo = args.BasicProperties.ReplyTo, CorrelationId = args.BasicProperties.CorrelationId },
                body: message);
        }
        finally
        {
            await callingChannel.BasicAckAsync(args.DeliveryTag, false);
        }
    }

    private static Task<T?> DeserializeBody<T>(ReadOnlyMemory<byte> body) where T : class
    {
        var idx = 0;
        for (var i = body.Length - 1; i >= 0; i--)
        {
            if (body.Span[i] != 0)
            {
                idx = i;
                break;
            }
        }

        var slice = body[..(idx + 1)].ToArray();
        if (typeof(T) == typeof(string))
        {
            return Task.FromResult(Encoding.UTF8.GetString(slice) as T);
        }

        using var stream = new MemoryStream(slice);
        stream.Seek(0, SeekOrigin.Begin);
        return JsonSerializer.DeserializeAsync<T>(stream).AsTask();
    }
}
