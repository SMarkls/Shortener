using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StatisticService.RabbitMqUtils;

namespace StatisticService;

public static class DependencyInjection
{
    public static void ConfigureRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionFactory = new ConnectionFactory
        {
            Password = configuration["RabbitMq:Password"],
            HostName = configuration["RabbitMq:Hostname"],
            UserName = configuration["RabbitMq:User"],
            Port = Convert.ToInt32(configuration["RabbitMq:Port"]),
            DispatchConsumersAsync = true
        };

        services.AddSingleton(connectionFactory);
        services.AddSingleton<RabbitMqConsumer>();
        services.AddSingleton<ChannelManager>();
    }
}
