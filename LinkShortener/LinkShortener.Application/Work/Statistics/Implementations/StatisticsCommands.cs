using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Work.Statistics.Interfaces;
using LinkShortener.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Work.Statistics.Implementations;

public class StatisticsCommands : IStatisticsCommands
{
    private readonly IApplicationDbContext context;

    public StatisticsCommands(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<long> CreateAsync(HttpContext httpContext, string token)
    {
        var link = await context.Links.FirstOrDefaultAsync(x => x.Token == token);
        if (link is null)
        {
            throw new NotFoundException($"Ссылка с токеном {token} не найдена");
        }

        var statistic = new Statistic
        {
            Browser = httpContext.Request.Headers["User-Agent"],
            ShortenLink = link,
            IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
            Time = DateTime.Now
        };
        await context.Statistics.AddAsync(statistic);
        await context.SaveChangesAsync();
        return statistic.Id;
    }
}