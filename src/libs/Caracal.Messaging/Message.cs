namespace Caracal.Messaging;

public readonly struct Message
{
    public Message(byte[] payload, Topic topic, Dictionary<string, string>? metadata = null)
    {
        Payload = payload;
        Topic = topic;
        Metadata = metadata ?? new Dictionary<string, string>();
    }

    public required byte[] Payload { get; init; }
    public required Topic Topic { get; init; }
    internal Topic? ResponseTopic { get; init; }
    public Dictionary<string, string> Metadata { get; init; }
}