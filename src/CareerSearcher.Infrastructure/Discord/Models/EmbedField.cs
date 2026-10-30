namespace CareerSearcher.Infrastructure.Discord.Models;

internal sealed class EmbedField
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool Inline { get; set; }

    public EmbedField(string name, string value, bool inline)
    {
        Name = name;
        Value = value;
        Inline = inline;
    }
}
