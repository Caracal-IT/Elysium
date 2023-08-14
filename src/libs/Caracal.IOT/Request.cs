namespace Caracal.IOT;

public sealed record Request
{
    public required byte[] Payload { get; init; }
}