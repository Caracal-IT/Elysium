namespace Caracal.Messaging;

public readonly struct Topic
{
    public Topic()
    {
        Name = "Unknown";
        QualityOfServiceLevel = 0x01;
        Retain = false;
    }

    public required string Name { get; init; }

    public bool Retain { get; init; } = false;

    public int QualityOfServiceLevel { get; init; }
}