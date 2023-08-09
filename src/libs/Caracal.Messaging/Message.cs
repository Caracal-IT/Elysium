namespace Caracal.Messaging;

public sealed record Message
{
    public required byte[] Payload { get; init; }
    public required Topic Topic { get; init; }
    internal Topic? ResponseTopic { get; init; }
}