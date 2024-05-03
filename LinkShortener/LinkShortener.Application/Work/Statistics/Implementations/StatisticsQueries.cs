using System.Text.RegularExpressions;
using LinkShortener.Application.Common;
using LinkShortener.Application.Models.Statistics;
using LinkShortener.Application.Work.Statistics.Interfaces;
using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Work.Statistics.Implementations;

public class StatisticsQueries : IStatisticsQueries
{
    private readonly IApplicationDbContext context;

    public StatisticsQueries(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<StatisticVM> GetStatisticsAsync(long shortenLinkId)
    {
        var statistics = await context.Statistics.Include(x => x.ShortenLink).Where(x => x.ShortenLink.Id == shortenLinkId).ToListAsync();
        var compressStatistic = CompressStatistic(statistics);
        var countUnique = statistics.Select(x => new { x.IpAddress, x.Browser }).Distinct().Count();
        return new StatisticVM { Statistics = compressStatistic, CountOfUniqueUsers = countUnique };
    }

    private static IList<CompressedStatistic> CompressStatistic(List<Statistic> statistics)
    {
        var list = new List<CompressedStatistic>(statistics.Count);
        statistics.ForEach(x =>
        {
            list.Add(new CompressedStatistic
            {
                Browser = GetBrowserName(x.Browser),
                Time = x.Time
            });
        });

        return list;
    }

    private static string GetBrowserName(string? userAgent)
    {
        var browser = "another";
        if (userAgent is null)
        {
            return browser;
        }

        browser = Regex.IsMatch(userAgent, "ucbrowser", RegexOptions.IgnoreCase) ? "UCBrowser" : browser;
        browser = Regex.IsMatch(userAgent, "edg", RegexOptions.IgnoreCase) ? "Edge" : browser;
        browser = Regex.IsMatch(userAgent, "googlebot", RegexOptions.IgnoreCase) ? "GoogleBot" : browser;
        browser = Regex.IsMatch(userAgent, "chromium", RegexOptions.IgnoreCase) ? "Chromium" : browser;
        browser = Regex.IsMatch(userAgent, "firefox|fxios", RegexOptions.IgnoreCase) && !Regex.IsMatch(userAgent, "seamonkey", RegexOptions.IgnoreCase) ? "Firefox" : browser;
        browser = Regex.IsMatch(userAgent, "; msie|trident", RegexOptions.IgnoreCase) && !Regex.IsMatch(userAgent, "ucbrowser", RegexOptions.IgnoreCase) ? "IE" : browser;
        browser = Regex.IsMatch(userAgent, "chrome|crios", RegexOptions.IgnoreCase) &&
                  !Regex.IsMatch(userAgent, "opr|opera|chromium|edg|ucbrowser|googlebot", RegexOptions.IgnoreCase)
            ? "Chrome"
            : browser;
        browser = Regex.IsMatch(userAgent, "safari", RegexOptions.IgnoreCase) &&
                  !Regex.IsMatch(userAgent, "chromium|edg|ucbrowser|chrome|crios|opr|opera|fxios|firefox", RegexOptions.IgnoreCase)
            ? "Safari"
            : browser;
        browser = Regex.IsMatch(userAgent, "opr|opera", RegexOptions.IgnoreCase) ? "Opera" : browser;

        return browser;
    }
}