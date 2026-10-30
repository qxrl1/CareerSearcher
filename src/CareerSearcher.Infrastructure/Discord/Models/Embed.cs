namespace CareerSearcher.Infrastructure.Discord.Models;

internal sealed class Embed
{
    public string Title { get; set; }
    public string Url { get; set; }
    public int Color { get; set; }
    public EmbedThumbnail? Thumbnail { get; set; } = new();
    public List<EmbedField> Fields { get; set; } = [];
}
