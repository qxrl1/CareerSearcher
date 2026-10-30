using CareerSearcher.Infrastructure.Discord.Models;

namespace CareerSearcher.Infrastructure.Discord.Builder;

internal sealed class DiscordEmbedPayloadBuilder
{
    private readonly WebhookPayload _webhookPayload = new();

    public WebhookPayload Build() => _webhookPayload;

    public DiscordEmbedPayloadBuilder SetContent(string content)
    {
        _webhookPayload.Content = content;
        return this;
    }

    public DiscordEmbedPayloadBuilder SetEmbed(Embed embed)
    {
        _webhookPayload.Embeds.Add(embed);
        return this;
    }
}
