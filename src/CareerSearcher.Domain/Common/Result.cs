namespace CareerSearcher.Domain.Common;

public class Result
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; set; }

    protected Result(bool success)
    {
        IsSuccess   = success;
    }

    protected Result(Error error)
    {
        IsSuccess   = false;
        Error       = error;
    }

    public static Result Success() => new(true);
    public static Result Failure(Error error) => new(error);

    public static implicit operator Result(Error error) => Failure(error);
}

public class Result<TResult> : Result
{
    public TResult? Value { get; set; }

    protected Result(TResult value) : base(true)
    {
        Value = value;
    }

    protected Result(Error error) : base(error)
    {
        Value = default;
    }

    public static Result<TResult> Success(TResult result) => new(result);
            
    public static implicit operator Result<TResult>(TResult result) => new(result);
    public static implicit operator Result<TResult>(Error error) => new(error);
}
