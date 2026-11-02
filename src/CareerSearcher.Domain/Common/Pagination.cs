namespace CareerSearcher.Domain.Common;

public class Pagination<T> where T : notnull
{
    public int Page                     { get; set; }
    public int PageSize                 { get; set; }
    public int Amount                   { get; set; }
    public int Pages                    { get; set; }
    public int TotalInPage              { get; set; }
    public ICollection<T> Data          { get; set; } = [];

    public Pagination(int page, int pageSize, int amount, ICollection<T> data)
    {
        Page        = page;
        PageSize    = pageSize;
        Amount      = amount;
        Pages       = (amount + pageSize - 1) / pageSize;
        Data        = data;
    }
}
