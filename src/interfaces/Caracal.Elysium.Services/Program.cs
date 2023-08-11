using System.Diagnostics.CodeAnalysis;
using Caracal.Elysium.IOT.Application.Consumers;
using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.Elysium.Services;
using Caracal.Elysium.Services.Mocks;
using Caracal.Elysium.Services.Services;
using Caracal.IOT;
using Caracal.Messaging;
using Caracal.Messaging.Routing;
using Caracal.Messaging.Routing.Config;
using MassTransit;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using IRouter = Caracal.Messaging.Routing.IRouter;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddSingleton(GetConfiguration(args))
       .ConfigureOptions<RoutingOptions>()
       .AddSerilog(cfg =>
       {
            cfg.ReadFrom.Configuration(builder.Configuration);
            cfg.Enrich.WithCallerInfo(
                includeFileInfo: true, 
                assemblyPrefix: "Caracal.");
            cfg.Enrich.WithProperty("AppName", builder.Configuration["AppName"]);
            cfg.Enrich.WithProperty("HostingLocation", builder.Configuration["HostingLocation"]);
            cfg.Enrich.FromLogContext();
       })
       .AddSingleton<IGateway, MockGateway>()
       .AddSingleton<IGatewayProducer, GatewayProducerWithLogger>()
       .AddSingleton<IRouteingFactory, RouteingFactory2>()
       .AddSingleton<IRouter, Router>()
       .AddSingleton<IWriteOnlyClient>(serviceProvider => serviceProvider.GetRequiredService<IRouter>());

builder.Services
       .AddMassTransit(x =>
       {
            x.AddConsumers(typeof(IConsumerMarker).Assembly);
            x.UsingInMemory((context, cfg) => {
                cfg.ConfigureEndpoints(context);
            });
       });

builder.Services.AddHostedService<DefaultWorkerService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapGet("/", () => "Hello World!");

app.Run();

static IConfigurationRoot GetConfiguration(string[] args) => 
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

[ExcludeFromCodeCoverage]
public partial class Program { }