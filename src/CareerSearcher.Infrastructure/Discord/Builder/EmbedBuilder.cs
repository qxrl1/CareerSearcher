using CareerSearcher.Infrastructure.Discord.Models;

namespace CareerSearcher.Infrastructure.Discord.Builder;

internal sealed class EmbedBuilder
{
    private Embed _embed = new();

    public Embed Build() => _embed;

    public EmbedBuilder SetTitle(string title)
    {
        _embed.Title = title;
        return this;
    }

    public EmbedBuilder SetUrl(string url)
    {
        _embed.Url = url;
        return this;
    }

    public EmbedBuilder SetColor(int color)
    {
        _embed.Color = color;
        return this;
    }

    public EmbedBuilder SetThumbnail(string thumbnailUrl)
    {
        _embed.Thumbnail!.Url = thumbnailUrl;
        return this;
    }

    public EmbedBuilder SetFields(EmbedField field)
    {
        _embed.Fields.Add(field);
        return this;
    }

    public EmbedBuilder AddField(string name, string value, bool inline)
    {
        _embed.Fields.Add(new EmbedField(name, value, inline));
        return this;
    }
}
