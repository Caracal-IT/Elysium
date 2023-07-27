using FluentAssertions;

namespace Caracal.Lang.Tests.Unit;

public sealed class ResultTests
{
    [Fact]
    public void ImplicitOperator_ShouldCreateResultWithValue()
    {
        Result<int> result = 42;
        
        result.Value.Should().Be(42);
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFaulted.Should().BeFalse();
    }
    
    [Fact]
    public void ImplicitOperator_ShouldCreateResultWithException()
    {
        var exception = new Exception();
        Result<int> result = exception;
        
        result.Value.Should().Be(default);
        result.Exception.Should().Be(exception);
        result.IsSuccess.Should().BeFalse();
        result.IsFaulted.Should().BeTrue();
    }
}