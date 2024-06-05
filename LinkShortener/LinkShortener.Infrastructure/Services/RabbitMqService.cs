using System.Text;
using System.Text.Json;
using LinkShortener.Application.Models.Statistics;
using LinkShortener.Application.Work.Statistics.Interfaces;
using LinkShortener.Domain.Entities;
using LinkShortener.Infrastructure.Utils;
using RabbitMQ.Client;

namespace LinkShortener.Infrastructure.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly RabbitMqChannelManager manager;

    public RabbitMqService(RabbitMqChannelManager manager)
    {
        this.manager = manager;
    }

    public async Task PublishStatisticAsync(Statistic body)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, body, typeof(Statistic));
        var message = stream.GetBuffer();
        var ch = await manager.GetChannel();
        await ch.BasicPublishAsync("statistics", string.Empty, body: message);
    }

    public async Task<StatisticVM> GetStatisticsAsync(string token)
    {
        var channel = await manager.GetRpcCallingChannel();
        var correlationId = Guid.NewGuid().ToString();
        var body = Encoding.UTF8.GetBytes(token);
        var tcs = new TaskCompletionSource<StatisticVM>();
        if (!RabbitMqChannelManager.CallbackMapper.TryAdd(correlationId, tcs))
        {
            throw new Exception("Can't call procedure.");
        }

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "rpc_get_statistic",
            new BasicProperties { CorrelationId = correlationId, ReplyTo = "rpc_get_statistic" },
            body: body);

        return await tcs.Task;
    }
}