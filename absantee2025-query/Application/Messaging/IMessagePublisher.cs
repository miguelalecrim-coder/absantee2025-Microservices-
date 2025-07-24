
using Domain.Models;

namespace Application.Messaging
{

    public interface IMessagePublisher
    {
        Task PublishCreatedProjectMessageAsync(Guid Id, string Title, string Acronym, PeriodDate periodDate);

        Task PublishUpdatedProjectMessageAsync(Guid Id, string Title, string Acronym, PeriodDate periodDate);
    }
}