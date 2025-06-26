
using Domain.Models;

namespace WebApi.Messages;

public record ProjectUpdatedMessage(Guid id, string title, string acronym, PeriodDate periodDate);