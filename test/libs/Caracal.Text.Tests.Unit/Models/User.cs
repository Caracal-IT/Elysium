namespace Caracal.Text.Tests.Unit.Models;

public sealed class User
{
    public static readonly User Default = new () { FirstName = "John", LastName = "Doe", Age = 42 };
    public static readonly User DefaultWithAddress = new () { FirstName = "John", LastName = "Doe", Age = 42, Address = new Address { Street = "Main", Number = 123 } };
    
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int Age { get; init; }
    public Address? Address { get; init; }
}