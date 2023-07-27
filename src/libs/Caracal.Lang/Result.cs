namespace Caracal.Lang;

public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    public Result(T value)
    {
        _value = value;
        _exception = null;
    }
    
    public Result(Exception exception)
    {
        _value = default;
        _exception = exception;
    }
    
    public static implicit operator Result<T>(T value) =>
        new (value);
}