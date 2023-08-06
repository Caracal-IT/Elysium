using System.Diagnostics.CodeAnalysis;
using Caracal.Elysium.IOT.Application.Consumers;
using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.Elysium.Services.Mocks;
using Caracal.Elysium.Services.Services;
using Caracal.IOT;
using Caracal.Messaging;
using Caracal.Messaging.Mqtt;
using MassTransit;
using Serilog;
using Serilog.Enrichers.CallerInfo;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSerilog(cfg =>
    {
        cfg.ReadFrom.Configuration(builder.Configuration);
        cfg.Enrich.WithProperty("AppName", builder.Configuration["AppName"]);
        cfg.Enrich.WithProperty("HostingLocation", builder.Configuration["HostingLocation"]);
        cfg.Enrich.WithCallerInfo(true, new List<string> { "Caracal.Elysium.Services", "Caracal.Elysium.IOT" });
        cfg.Enrich.FromLogContext();
    })
    .AddSingleton<IGateway, MockGateway>()
    .AddSingleton<IGatewayProducer, GatewayProducerWithLogger>()
    .AddSingleton<IWriteOnlyClient>(_ =>
    {
        var connectionString = new MqttConnectionString();
        var connection = new MqttConnection(connectionString);
        return new MqttWriteOnlyClient(connection);
    });

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

[ExcludeFromCodeCoverage]
public partial class Program { }