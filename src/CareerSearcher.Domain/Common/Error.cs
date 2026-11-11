namespace CareerSearcher.Domain.Common;

public enum ErrorType 
{
    NotFound,
    Validation,
    Unauthorized,
    BadRequest
}

public sealed record Error(ErrorType ErrorType, string Id, string Description)
{
    public static readonly Error NotFound = new(ErrorType.NotFound, "NotFound", "Not Found"); 
}
