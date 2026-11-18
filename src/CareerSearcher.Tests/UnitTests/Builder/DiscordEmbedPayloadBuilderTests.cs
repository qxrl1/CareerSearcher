using CareerSearcher.Infrastructure.Discord.Builder;
using Xunit;

namespace CareerSearcher.Tests.UnitTests.Builder;

[TestClass]
public class DiscordEmbedPayloadBuilderTests
{
    [Fact]
    public void Should_Build_An_Embed_Payload()
    {
        var embed = new EmbedBuilder()
            .SetTitle("embed_title")
            .SetUrl("embed_url")
            .SetColor(13211188)
            .SetThumbnail("thumbnail")
            .AddField("field1", "field1value", true)
            .Build();

        var payload = new DiscordEmbedPayloadBuilder()
            .SetContent("test")
            .SetEmbed(embed)
            .Build();

        Assert.NotNull(payload);
        Assert.Equal("test", payload.Content);
        Assert.NotNull(payload.Embeds);
        Assert.NotEmpty(payload.Embeds);
    }
}
