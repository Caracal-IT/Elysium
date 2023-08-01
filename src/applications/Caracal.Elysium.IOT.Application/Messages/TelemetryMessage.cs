namespace Caracal.Elysium.IOT.Application.Messages;

public interface IMessage{}
public sealed class TelemetryMessage: IMessage
{
    public string Payload { get; set; } = string.Empty;
}

public sealed class TelemetryErrorMessage: IMessage
{
    public string Payload { get; set; } = string.Empty;
}