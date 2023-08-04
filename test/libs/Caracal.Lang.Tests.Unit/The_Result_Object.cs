using FluentAssertions;
// ReSharper disable InconsistentNaming
namespace Caracal.Lang.Tests.Unit;

[Trait("Category","Unit")]
public sealed class The_Result_Object
{
    [Fact]
    public void Should_Wrap_The_Value_As_Successful()
    {
        var result = new Result<int>(42);
        
        result.Value.Should().Be(42);
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFaulted.Should().BeFalse();
    }
    
    [Fact]
    public void Should_Wrap_The_Exception_As_A_Faulted()
    {
        var exception = new Exception();
        var result = new Result<int>(exception);
        
        result.Value.Should().Be(default);
        result.Exception.Should().Be(exception);
        result.IsSuccess.Should().BeFalse();
        result.IsFaulted.Should().BeTrue();
    }
    
    [Fact]
    public void Should_Match_The_Successful_Result()
    {
        var result = new Result<int>(42).Match(value => value, null!);

        result.Should().Be(42);
    }
    
    [Fact]
    public void  Should_Match_The_Faulted_Result()
    {
        var exception = new Exception();
        var result = new Result<int>(exception).Match(null!, ex =>  ex);
        
        result.Should().Be(exception);
    }

    [Fact]
    public void Should_Implicit_Wrap_A_Successful_Value()
    {
        Result<int> result = 42;
        
        result.Value.Should().Be(42);
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFaulted.Should().BeFalse();
    }
    
    [Fact]
    public void Should_Implicit_Wrap_An_Exception()
    {
        var exception = new Exception();
        Result<int> result = exception;
        
        result.Value.Should().Be(default);
        result.Exception.Should().Be(exception);
        result.IsSuccess.Should().BeFalse();
        result.IsFaulted.Should().BeTrue();
    }
    
    [Fact]
    public void Should_Match_The_Successful_Result_With_Action()
    {
        var result = new Result<int>(42);
        var value = 0;
        Action<Exception> faulted = null!;
        
        result.Match(Success, faulted);
        value.Should().Be(42);
        return;
        void Success(int v) => value = v;
    }
    
    [Fact]
    public void  Should_Match_The_Faulted_Result_With_Action()
    {
        var exception = new Exception();
        var result = new Result<int>(exception);
        Action<int> success = null!;
        Exception? faulted = null;
        
        result.Match(success, ex => faulted = ex);
        
        faulted.Should().Be(exception);
    }
}