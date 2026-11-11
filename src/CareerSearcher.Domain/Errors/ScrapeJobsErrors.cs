using CareerSearcher.Domain.Common;

namespace CareerSearcher.Domain.Errors;

public static class ScrapeJobsErrors
{
    public static readonly Error EmptyQuery = new(ErrorType.BadRequest, "EmptyQuery", "Query can't be empty.");
}
