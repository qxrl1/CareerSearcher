using CareerSearcher.Application.Interfaces;
using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Infrastructure.Notification;

internal sealed class JobNotificationQueue : IJobNotificationQueue
{
    private readonly Queue<Job> _queue = new();

    public bool Dequeue(out Job? outJob)
    {
        outJob = null;
        return _queue.TryDequeue(out outJob);
    }

    public void EnqueueNotification(Job job)
    {
        _queue.Enqueue(job);
    }
}
