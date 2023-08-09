namespace Caracal.IOT;

public sealed record Response
{
    public required byte[] Payload { get; init; }
}