using Microsoft.Extensions.Hosting;
using StatisticService.RabbitMqUtils;

namespace StatisticService;

public class HostedService : IHostedService
{
    private readonly ChannelManager _channelManager;
    private readonly RabbitMqConsumer _consumer;

    public HostedService(ChannelManager channelManager, RabbitMqConsumer consumer)
    {
        _channelManager = channelManager;
        _consumer = consumer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _channelManager.SetHandlers(_consumer.MessageReceived, _consumer.GetStatisticsProcedureCalled);
        await _channelManager.StartConsuming();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _channelManager.StopConsuming();
    }
}
