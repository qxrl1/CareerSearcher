namespace CareerSearcher.Domain.Entities;

public sealed class Job : BaseEntity
{
    public string Name { get; set; }
    public string Company { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string UniqueId { get; }

    private Job() { }

    public Job(string name, string company, string description, string url)
    {
        Name        = name;
        Company     = company;
        Description = description;
        Url         = url;

        UniqueId    = $"{Name}+{Company}";
    }
}
