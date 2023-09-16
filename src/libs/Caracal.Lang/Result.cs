namespace Caracal.Lang;

public sealed record Result<T>
{
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

    public T? Value { get; }
    public Exception? Exception { get; }

    private ResultState State => Exception is null ? ResultState.Success : ResultState.Faulted;

    public bool IsSuccess => State == ResultState.Success;
    public bool IsFaulted => State == ResultState.Faulted;

    public TS Match<TS>(Func<T, TS> onSuccess, Func<Exception, TS> onFaulted)
    {
        return IsSuccess ? onSuccess(Value!) : onFaulted(Exception!);
    }

    public void Match(Action<T> onSuccess, Action<Exception> onFaulted)
    {
        if (IsSuccess)
            onSuccess(Value!);
        else
            onFaulted(Exception!);
    }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Exception value)
    {
        return new Result<T>(value);
    }
}

public enum ResultState : byte
{
    Faulted,
    Success
}