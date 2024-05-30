namespace StatisticService.Models;

public class StatisticVm
{
    public required IList<CompressedStatistic> Statistics { get; init; }
    public required int CountOfUniqueUsers { get; init; }
}
