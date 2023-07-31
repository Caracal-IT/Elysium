using FluentAssertions;

namespace Caracal.Lang.Tests.Unit;

[Trait("Category","Unit")]
public sealed class ResultTests
{
    [Fact]
    public void ShouldCreateResultWithValue()
    {
        var result = new Result<int>(42);
        
        result.Value.Should().Be(42);
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFaulted.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldCreateResultWithException()
    {
        var exception = new Exception();
        var result = new Result<int>(exception);
        
        result.Value.Should().Be(default);
        result.Exception.Should().Be(exception);
        result.IsSuccess.Should().BeFalse();
        result.IsFaulted.Should().BeTrue();
    }
    
    [Fact]
    public void Match_ShouldCallOnSuccess()
    {
        var result = new Result<int>(42).Match(value => value, null!);

        result.Should().Be(42);
    }
    
    [Fact]
    public void Match_ShouldCallOnFaulted()
    {
        var exception = new Exception();
        var result = new Result<int>(exception).Match(null!, ex =>  ex);
        
        result.Should().Be(exception);
    }

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