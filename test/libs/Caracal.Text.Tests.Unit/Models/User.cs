namespace Caracal.Text.Tests.Unit.Models;

public class User
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int Age { get; init; }
    public Address? Address { get; init; }
}