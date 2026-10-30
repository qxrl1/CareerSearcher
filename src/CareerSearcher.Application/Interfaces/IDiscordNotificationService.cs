using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Application.Interfaces;

public interface IDiscordNotificationService
{
    Task SendAsync(Job job, CancellationToken cancellationToken = default);
}
