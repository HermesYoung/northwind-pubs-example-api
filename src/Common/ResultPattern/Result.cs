namespace Common.ResultPattern;

public struct Result
{
    public bool IsSuccess { get; }
    public ErrorMessageBase? Error { get; }

    public Result()
    {
        IsSuccess = true;
    }

    public Result(ErrorMessageBase? error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(ErrorMessageBase value) => new(value);
    public static Result Success() => new();
    public static Result Failure(ErrorMessageBase value) => new(value);
}