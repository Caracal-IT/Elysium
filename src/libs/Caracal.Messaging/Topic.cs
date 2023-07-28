namespace Caracal.Messaging;

public readonly struct Topic
{
    public Topic()
    {
        Name = "Unknown";
        QualityOfServiceLevel = 1;
        Retain = false;
    }

    public required string Name { get; init; }

    public bool Retain { get; init; } = false;

    public int QualityOfServiceLevel { get; init; }
}