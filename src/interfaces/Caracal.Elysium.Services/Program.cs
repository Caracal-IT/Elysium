using System.Diagnostics.CodeAnalysis;
using Caracal.Elysium.IOT.Application.Consumers;
using Caracal.Elysium.IOT.Application.Ingress;
using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.Elysium.Services.Mocks;
using Caracal.Elysium.Services.Services;
using Caracal.IOT;
using Caracal.Messaging;
using Caracal.Messaging.Ingress;
using Caracal.Messaging.Ingress.Config;
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
    .ConfigureOptions<IngressOptions>()
    .AddSerilog(cfg =>
    {
        cfg.ReadFrom.Configuration(builder.Configuration);
        cfg.Enrich.WithCallerInfo(
            true,
            "Caracal.");
        cfg.Enrich.WithProperty("AppName", builder.Configuration["AppName"]);
        cfg.Enrich.WithProperty("HostingLocation", builder.Configuration["HostingLocation"]);
        cfg.Enrich.FromLogContext();
    })
    .AddSingleton<IGatewayRequest, MockGatewayRequest>()
    .AddSingleton<IGatewayProducer, GatewayProducerWithLogger>()
    .AddSingleton<IRoutingFactory, RoutingFactory>()
    .AddSingleton<IRouter, Router>()
    .AddSingleton<IWriteOnlyClient>(serviceProvider => serviceProvider.GetRequiredService<IRouter>())
    .AddSingleton<IIngressFactory, IngressFactory>()
    .AddSingleton<IGatewayCommand, MockGatewayCommand>()
    .AddSingleton<IIngressController, IngressController>();

builder.Services
    .AddMassTransit(x =>
    {
        x.AddConsumers(typeof(IConsumerMarker).Assembly);
        x.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
    });

builder.Services.AddHostedService<GatewayProducerWorkerService>();
builder.Services.AddHostedService<IngressWorkerService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapGet("/", () => "Hello World!");

app.Run();
return;

static IConfigurationRoot GetConfiguration(string[] args)
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();
}

[ExcludeFromCodeCoverage]
public partial class Program
{
}