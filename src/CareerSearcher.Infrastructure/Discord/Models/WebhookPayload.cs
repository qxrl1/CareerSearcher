namespace CareerSearcher.Infrastructure.Discord.Models;

internal sealed class WebhookPayload
{
    public string Content { get; set; } = string.Empty;
    public bool Tts { get; set; }
    public int Flags { get; set; }
    public List<Embed> Embeds { get; set; } = [];
}
