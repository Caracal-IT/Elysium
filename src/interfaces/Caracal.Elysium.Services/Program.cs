using Caracal.Elysium.IOT.Application.Consumers;
using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.Elysium.Services.Mocks;
using Caracal.Elysium.Services.Services;
using Caracal.IOT;
using Caracal.Messaging;
using Caracal.Messaging.Mqtt;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
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

app.MapGet("/", () => "Hello World!");

app.Run();