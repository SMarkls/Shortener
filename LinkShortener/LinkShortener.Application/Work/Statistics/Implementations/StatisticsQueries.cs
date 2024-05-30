using LinkShortener.Application.Models.Statistics;
using LinkShortener.Application.Work.Statistics.Interfaces;

namespace LinkShortener.Application.Work.Statistics.Implementations;

public class StatisticsQueries : IStatisticsQueries
{
    private readonly IRabbitMqService rabbitMqService;

    public StatisticsQueries(IRabbitMqService rabbitMqService)
    {
        this.rabbitMqService = rabbitMqService;
    }

    public Task<StatisticVM> GetStatisticsAsync(string token)
    {
        return rabbitMqService.GetStatisticsAsync(token);
    }
}