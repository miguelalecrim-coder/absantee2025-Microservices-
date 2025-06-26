

using Application.Messaging;
using Domain.Models;
using MassTransit;
using WebApi.Messages;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishCreatedProjectMessageAsync(Guid id, string title, string acronym, PeriodDate period)
    {
        await _publishEndpoint.Publish(new ProjectCreatedMessage(id, title, acronym, period));
    }

    public async Task PublishUpdatedProjectMessageAsync(Guid id, string title, string acronym, PeriodDate period)
    {
        await _publishEndpoint.Publish(new ProjectUpdatedMessage(id, title, acronym, period));
    }

}