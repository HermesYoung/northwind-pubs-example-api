namespace Common.ResultPattern;

public abstract class ErrorMessageBase(int code, string message)
{
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
}