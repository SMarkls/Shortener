using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StatisticService;
using StatisticService.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<HostedService>();
builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var host = builder.Build();
host.Run();
