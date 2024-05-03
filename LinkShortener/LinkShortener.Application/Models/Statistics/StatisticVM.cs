namespace LinkShortener.Application.Models.Statistics;

public class StatisticVM
{
    public required IList<CompressedStatistic> Statistics { get; init; }
    public required int CountOfUniqueUsers { get; init; }
}