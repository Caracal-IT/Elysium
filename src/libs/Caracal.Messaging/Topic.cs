namespace Caracal.Messaging;

public readonly record struct Topic
{
    public Topic()
    {
        Path = "Unknown";
        QualityOfServiceLevel = 0x01;
        Retain = false;
    }

    public required string Path { get; init; }

    public bool Retain { get; init; } = false;

    public int QualityOfServiceLevel { get; init; }
}