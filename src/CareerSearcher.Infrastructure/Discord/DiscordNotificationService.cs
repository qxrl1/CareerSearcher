using CareerSearcher.Application.Interfaces;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Infrastructure.Discord.Builder;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CareerSearcher.Infrastructure.Discord;

internal sealed class DiscordNotificationService : IDiscordNotificationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DiscordNotificationService> _logger;

    public DiscordNotificationService(HttpClient httpClient, ILogger<DiscordNotificationService> logger)
    {
        _httpClient = httpClient;
        _logger     = logger;
    }

    public async Task SendAsync(Job job, CancellationToken cancellationToken = default)
    {
        var embed = new EmbedBuilder()
            .AddField("Company", job.Company, false)
            .SetTitle(job.Name)
            .SetUrl(job.Url)
            .SetColor(13211188)
            .Build();

        var payload = new DiscordEmbedPayloadBuilder()
            .SetContent("@everyone")
            .SetEmbed(embed)
            .Build();

        var response = await _httpClient.PostAsJsonAsync("", payload, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
