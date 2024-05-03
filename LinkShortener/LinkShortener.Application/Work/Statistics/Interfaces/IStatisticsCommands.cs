using Microsoft.AspNetCore.Http;

namespace LinkShortener.Application.Work.Statistics.Interfaces;

public interface IStatisticsCommands
{
    Task<long> CreateAsync(HttpContext httpContext, string token);
}