using Microsoft.AspNetCore.Http;

namespace LinkShortener.Application.Work.Statistics.Interfaces;

public interface IStatisticsCommands
{
    Task CreateAsync(HttpContext httpContext, string token);
}