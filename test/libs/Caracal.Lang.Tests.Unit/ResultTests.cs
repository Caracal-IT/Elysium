using FluentAssertions;

namespace Caracal.Lang.Tests.Unit;

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
        var result = new Result<int>(42);
        var called = 0;
        
        result.Match(value => called = value, null!);
        
        called.Should().Be(result.Value);
    }
    
    [Fact]
    public void Match_ShouldCallOnFaulted()
    {
        var exception = new Exception();
        var result = new Result<int>(exception);
        var called = new Exception();
        
        result.Match(null!, ex => called = ex);
        
        called.Should().Be(exception);
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