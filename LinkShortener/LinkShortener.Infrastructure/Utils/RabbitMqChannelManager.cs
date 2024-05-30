using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using LinkShortener.Application.Models.Statistics;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace LinkShortener.Infrastructure.Utils;

public class RabbitMqChannelManager
{
    public static readonly ConcurrentDictionary<string, TaskCompletionSource<StatisticVM>> CallbackMapper = new();
    private readonly ConnectionFactory factory;
    private IConnection? connection;
    private IChannel? channel;
    private IChannel? rpcCallingChannel;
    private IChannel? rpcReturningChannel;

    public RabbitMqChannelManager(ConnectionFactory factory)
    {
        this.factory = factory;
    }

    private async ValueTask<IConnection> GetConnection()
    {
        if (connection is null || !connection.IsOpen)
        {
            Log.Verbose("Rabbit Mq Connection was destroyed. Creating new");
            connection = await factory.CreateConnectionAsync();
            await ConfigureReturningChannel();
        }

        return connection;
    }

    private async Task ConfigureReturningChannel()
    {
        var channel = await GetRpcReturningChannel();
        await channel.QueueDeclareAsync("rpc_return_statistic", true, false, false);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += RpcReturned;
        await channel.BasicConsumeAsync("rpc_return_statistic", true, consumer);
    }

    private static async Task RpcReturned(object sender, BasicDeliverEventArgs ea)
    {
        if (!CallbackMapper.TryRemove(ea.BasicProperties.CorrelationId ?? string.Empty, out var tcs))
        {
            Log.Error("Unknown correlationId {BasicPropertiesCorrelationId}", ea.BasicProperties.CorrelationId);
            return;
        }

        try
        {
            var statisticVm = await DeserializeBody<StatisticVM>(ea.Body);
            if (statisticVm is null)
            {
                tcs.SetException(new Exception("Can't get data"));
                return;
            }

            tcs.SetResult(statisticVm);
        }
        catch (Exception ex)
        {
            tcs.SetException(ex);
        }
        finally
        {
            tcs.SetCanceled();
        }
    }

    public async ValueTask<IChannel> GetChannel()
    {
        if (channel is null || channel.IsClosed)
        {
            Log.Warning("Rabbit Mq Channel was destroyed. Creating new");
            var conn = await GetConnection();
            channel = await conn.CreateChannelAsync();
        }

        return channel;
    }

    public async ValueTask<IChannel> GetRpcCallingChannel()
    {
        if (rpcCallingChannel is null || rpcCallingChannel.IsClosed)
        {
            Log.Warning("Rabbit mq rpc channel was destroyed. Creating new");
            var conn = await GetConnection();
            rpcCallingChannel = await conn.CreateChannelAsync();
        }

        return rpcCallingChannel;
    }

    public async ValueTask<IChannel> GetRpcReturningChannel()
    {
        if (rpcReturningChannel is null || rpcReturningChannel.IsClosed)
        {
            Log.Warning("Rabbit mq rpc channel was destroyed. Creating new");
            var conn = await GetConnection();
            rpcReturningChannel = await conn.CreateChannelAsync();
        }

        return rpcReturningChannel;
    }


    private static Task<T?> DeserializeBody<T>(ReadOnlyMemory<byte> body) where T : class
    {
        var idx = 0;
        for (int i = body.Length - 1; i >= 0; i--)
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