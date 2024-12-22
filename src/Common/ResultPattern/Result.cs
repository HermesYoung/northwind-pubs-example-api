namespace Common.ResultPattern;

public readonly struct Result
{
    public bool IsSuccess { get; }
    public ErrorMessageBase? Error => !IsSuccess ? _error : throw new InvalidOperationException();
    private readonly ErrorMessageBase? _error;

    public Result()
    {
        IsSuccess = true;
    }

    private Result(ErrorMessageBase? error)
    {
        IsSuccess = false;
        _error = error;
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(ErrorMessageBase value) => new(value);
    public static Result Success() => new();
    public static Result Failure(ErrorMessageBase value) => new(value);
}