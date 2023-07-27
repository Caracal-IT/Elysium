namespace Caracal.Lang;

public readonly struct Result<T>
{
    public T? Value { get;  }
    public Exception? Exception { get; }

    private ResultState State => Exception is null ? ResultState.Success : ResultState.Faulted;
    
    public bool IsSuccess => State == ResultState.Success;
    public bool IsFaulted => State == ResultState.Faulted;

    public Result(T value)
    {
        Value = value;
        Exception = null;
    }
    
    public Result(Exception exception)
    {
        Value = default;
        Exception = exception;
    }
    
    public void Match(Action<T> onSuccess, Action<Exception> onFaulted)
    {
        if (IsSuccess)
            onSuccess(Value!);
        else
            onFaulted(Exception!);
    }
    
    public static implicit operator Result<T>(T value) =>
        new (value);
    
    public static implicit operator Result<T>(Exception value) =>
        new (value);
}

public enum ResultState : byte
{
    Faulted,
    Success
}