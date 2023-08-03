// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Caracal.Text.Tests.Unit.Models;

public sealed class Address
{
    public string Street { get; init; } = string.Empty;
    public int Number { get; init; }
}