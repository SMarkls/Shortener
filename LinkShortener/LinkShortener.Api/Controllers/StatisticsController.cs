using LinkShortener.Application.Models.Statistics;
using LinkShortener.Application.Work.Statistics.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class StatisticsController : Controller
{
    private readonly IStatisticsQueries statisticsQueries;

    public StatisticsController(IStatisticsQueries statisticsQueries)
    {
        this.statisticsQueries = statisticsQueries;
    }

    [HttpGet]
    public async Task<StatisticVM> GetStatistics(int shortenLinkId)
    {
        return await statisticsQueries.GetStatisticsAsync(shortenLinkId);
    }
}