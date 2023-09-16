namespace Caracal.Elysium.IOT.Application.Messages;

public sealed record TelemetryMessage(byte[] Payload);

public sealed record TelemetryErrorMessage(byte[] Payload);