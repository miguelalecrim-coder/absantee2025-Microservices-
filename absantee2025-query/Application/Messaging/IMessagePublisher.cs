
using Domain.Models;

namespace Application.Messaging
{

    public interface IMessagePublisher
    {
        Task PublishCreatedProjectMessageAsync(Guid id, string title, string acronym, PeriodDate periodDate);

        Task PublishUpdatedProjectMessageAsync(Guid id, string title, string acronym, PeriodDate periodDate);
    }
}