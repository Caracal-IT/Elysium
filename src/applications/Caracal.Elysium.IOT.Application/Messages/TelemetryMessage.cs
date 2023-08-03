namespace Caracal.Elysium.IOT.Application.Messages;

public interface IMessage{}
public sealed class TelemetryMessage: IMessage
{
    public byte[] Payload { get; init; } = Array.Empty<byte>();
}

public sealed class TelemetryErrorMessage: IMessage
{
    public byte[] Payload { get; init; } = Array.Empty<byte>();
}