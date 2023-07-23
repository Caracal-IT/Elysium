namespace Caracal.Text.Tests.Unit.Models;

public class User
{
    public static User Default = new () { FirstName = "John", LastName = "Doe", Age = 42 };
    public static User DefaultWithAddress = new () { FirstName = "John", LastName = "Doe", Age = 42, Address = new Address { Street = "Main", Number = 123 } };
    
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int Age { get; init; }
    public Address? Address { get; init; }
}