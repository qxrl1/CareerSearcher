using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Application.Interfaces;

public interface IJobNotificationQueue
{
    void EnqueueNotification(Job job);
    bool Dequeue(out Job? outJob);
}
