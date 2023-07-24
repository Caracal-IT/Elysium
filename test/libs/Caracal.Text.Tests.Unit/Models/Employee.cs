using System.Xml.Serialization;

namespace Caracal.Text.Tests.Unit.Models;

public class Employee
{
    public static readonly Employee Default = new () { Id = string.Empty, FirstName = "John", Surname = "Doe"};
    public static readonly Employee Complex = new () { Id = "MockId1", FirstName = "John", Surname = "Doe", Roles = new() { "Admin", "Employee" }};

    [XmlAttribute("Id")]
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    
    public string? EmployeeNumber { get; set; }
    
    [XmlArrayItem("Role")]
    public List<string> Roles { get; set; } = new ();
} 