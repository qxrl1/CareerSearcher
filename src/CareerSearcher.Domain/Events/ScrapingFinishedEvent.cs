using CareerSearcher.Domain.Entities;
using MediatR;

namespace CareerSearcher.Domain.Events;

public sealed record ScrapingFinishedEvent(IReadOnlyCollection<Job> Jobs) : INotification;

