﻿using System.Xml.Serialization;
using Caracal.Text.Tests.Unit.Utils;

namespace Caracal.Text.Tests.Unit.Models;

public class Employee
{
    public static readonly Employee Default = new () { Id = string.Empty, FirstName = "John", Surname = "Doe"};
    public static readonly Employee Complex = new () {
        Id = "MockId1", 
        FirstName = "John", 
        Surname = "Doe",
        EmployeeNumber = HashUtility.Hash256("MockId1"),
        Roles = new() { "Admin", "Employee" }
    };

    [XmlAttribute("Id")]
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    
    public string? EmployeeNumber { get; set; }
    
    [XmlArrayItem("Role")]
    public List<string> Roles { get; set; } = new ();
} 