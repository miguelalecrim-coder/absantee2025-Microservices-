
using Domain.Models;

namespace WebApi.Messages;

public record ProjectCreatedMessage( Guid id, string title, string acronym, PeriodDate periodDate);