namespace Caracal.Text.Tests.Unit.Models;

public class Employee
{
    public static readonly Employee Default = new () { FirstName = "John", Surname = "Doe"};
    public string FirstName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
}