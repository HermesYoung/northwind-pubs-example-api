namespace Common.ResultPattern;

public readonly struct Result<TValue>
{
    public bool IsSuccess { get; }
    public TValue? Value => IsSuccess ? _value : throw new InvalidOperationException();
    public ErrorMessageBase? Error => !IsSuccess ? _error : throw new InvalidOperationException();

    private readonly TValue? _value;
    private readonly ErrorMessageBase? _error;

    public Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
    }

    public Result(ErrorMessageBase error)
    {
        IsSuccess = false;
        _error = error;
    }
}