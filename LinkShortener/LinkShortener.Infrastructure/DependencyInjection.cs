using System.Text;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Services;
using LinkShortener.Domain.Identity.Entities;
using LinkShortener.Infrastructure.Persistence;
using LinkShortener.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;

namespace LinkShortener.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdentityTokenService, IdentityTokenService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        var tokenValidationParametres = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new Exception("Jwt Key not found in config file")))
        };
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParametres;
            });
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddRedis(configuration);
        services.ConfigureSerilog(configuration);
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "shortener_";
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { configuration.GetConnectionString("Redis") ?? throw new Exception("No redis connection string") },
                ConnectRetry = 5,
                ReconnectRetryPolicy = new LinearRetry(1500),
                ConnectTimeout = 5000,
                SyncTimeout = 0,
                DefaultDatabase = 0
            };
        });
    }

    private static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        var consoleLogLevel = Enum.Parse(typeof(LogEventLevel), configuration["Serilog:LogEventLevel:Console"]) is LogEventLevel consoleLevel
            ? consoleLevel
            : throw new Exception("No console loglevel in configuration");

        var fileLogLevel = Enum.Parse(typeof(LogEventLevel), configuration["Serilog:LogEventLevel:File"]) is LogEventLevel fileLevel
            ? fileLevel
            : throw new Exception("No file loglevel in configuration");

        var template = configuration["Serilog:Template"] ?? throw new Exception("No logging template");
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(consoleLogLevel, template)
            .WriteTo.File(configuration["Serilog:FileNameTemplate"], rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: fileLogLevel, outputTemplate: template)
            .CreateLogger();

        services.AddSerilog(Log.Logger);
    }
}