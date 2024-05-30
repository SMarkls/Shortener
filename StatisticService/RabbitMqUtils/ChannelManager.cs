using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StatisticService.RabbitMqUtils;

public class ChannelManager
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _persistChannel;
    private IChannel? _rpcCallingChannel;
    private IChannel? _rpcReturningChannel;
    private AsyncEventHandler<BasicDeliverEventArgs>? _persistStatisticHandler;
    private AsyncEventHandler<BasicDeliverEventArgs>? _getStatisticProcedureCalled;

    public ChannelManager(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public Task StartConsuming()
    {
        return GetConnection().AsTask();
    }

    public async Task StopConsuming()
    {
        if (_persistChannel != null && _persistChannel.IsOpen)
        {
            await _persistChannel.CloseAsync();
            _persistChannel.Dispose();
        }

        if (_connection != null && _connection.IsOpen)
        {
            await _connection.CloseAsync();
            _connection?.Dispose();
        }
    }

    private async ValueTask<IConnection> GetConnection()
    {
        if (_connection is null || !_connection.IsOpen)
        {
            _connection = await _factory.CreateConnectionAsync();
            await ConfigureConnection();
        }

        return _connection;
    }

    public async Task SetHandlers(AsyncEventHandler<BasicDeliverEventArgs> persistHandler,
        AsyncEventHandler<BasicDeliverEventArgs> procedureHandler)
    {
        _persistStatisticHandler = persistHandler;
        _getStatisticProcedureCalled = procedureHandler;
        await GetConnection();
    }

    public async ValueTask<IChannel> GetPersistChannel()
    {
        if (_persistChannel is null || _persistChannel.IsClosed)
        {
            var conn = await GetConnection();
            _persistChannel = await conn.CreateChannelAsync();
        }

        return _persistChannel;
    }

    public async ValueTask<IChannel> GetRpcCallingChannel()
    {
        if (_rpcCallingChannel is null || _rpcCallingChannel.IsClosed)
        {
            var conn = await GetConnection();
            _rpcCallingChannel = await conn.CreateChannelAsync();
        }

        return _rpcCallingChannel;
    }

    public async ValueTask<IChannel> GetRpcReturningChannel()
    {
        if (_rpcReturningChannel is null || _rpcReturningChannel.IsClosed)
        {
            var conn = await GetConnection();
            _rpcReturningChannel = await conn.CreateChannelAsync();
        }

        return _rpcReturningChannel;
    }

    private async Task ConfigureConnection()
    {
        if (_persistStatisticHandler is null || _getStatisticProcedureCalled is null)
        {
            return;
        }

        var persistChannel = await GetPersistChannel();
        var consumer = new AsyncEventingBasicConsumer(persistChannel);
        consumer.Received += _persistStatisticHandler;
        await persistChannel.BasicConsumeAsync("statistics_queue", true, consumer);

        var rpcChannel = await GetRpcCallingChannel();
        var rpcConsumer = new AsyncEventingBasicConsumer(rpcChannel);
        rpcConsumer.Received += _getStatisticProcedureCalled;
        await rpcChannel.BasicConsumeAsync("rpc_get_statistic", false, rpcConsumer);
    }
}
