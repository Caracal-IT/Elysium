using Caracal.Elysium.IOT.Application.Consumers;
using Caracal.Elysium.IOT.Application.Producers;
using Caracal.Elysium.Services.Mocks;
using Caracal.Elysium.Services.Services;
using Caracal.IOT;
using Caracal.Messaging;
using Caracal.Messaging.Mqtt;
using MassTransit;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<IGateway, MockGateway>()
    .AddSingleton<IGatewayProducer, GatewayProducerWithLogger>()
    .AddSingleton<IWriteOnlyClient>(service =>
    {
        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
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