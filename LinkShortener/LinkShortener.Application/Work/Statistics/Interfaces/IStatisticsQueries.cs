using LinkShortener.Application.Models.Statistics;

namespace LinkShortener.Application.Work.Statistics.Interfaces;

public interface IStatisticsQueries
{
    Task<StatisticVM> GetStatisticsAsync(long shortenLinkId);
}