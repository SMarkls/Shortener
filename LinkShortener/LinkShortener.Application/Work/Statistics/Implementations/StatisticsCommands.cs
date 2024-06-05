using LinkShortener.Application.Work.Statistics.Interfaces;
using LinkShortener.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LinkShortener.Application.Work.Statistics.Implementations;

public class StatisticsCommands : IStatisticsCommands
{
    private readonly IRabbitMqService rabbitService;

    public StatisticsCommands(IRabbitMqService rabbitService)
    {
        this.rabbitService = rabbitService;
    }

    public async Task CreateAsync(HttpContext httpContext, string token)
    {
        var statistic = new Statistic
        {
            Browser = httpContext.Request.Headers["User-Agent"],
            Token = token,
            IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
            Time = DateTime.Now
        };

        await rabbitService.PublishStatisticAsync(statistic);
    }
}