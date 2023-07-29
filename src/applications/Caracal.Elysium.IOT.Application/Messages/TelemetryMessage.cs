namespace Caracal.Elysium.IOT.Application.Messages;

public interface IMessage{}
public class TelemetryMessage: IMessage
{
    public string Payload { get; set; } = string.Empty;
}

public class TelemetryErrorMessage: IMessage
{
    public string Payload { get; set; } = string.Empty;
}