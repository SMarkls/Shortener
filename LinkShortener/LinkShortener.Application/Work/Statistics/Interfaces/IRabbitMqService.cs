using LinkShortener.Application.Models.Statistics;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Application.Work.Statistics.Interfaces;

public interface IRabbitMqService
{
    Task PublishStatisticAsync(Statistic body);
    Task<StatisticVM> GetStatisticsAsync(string token);
}