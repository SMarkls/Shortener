using System.Reflection;
using LinkShortener.Application.Work.ShortenLinks.Implementations;
using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services
            .AddTransient<IShortenLinksCommands, ShortenLinksCommands>()
            .AddTransient<IShortenLinksQueries, ShortenLinksQueries>();
    }
}