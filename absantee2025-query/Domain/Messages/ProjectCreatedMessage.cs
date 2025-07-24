
using Domain.Models;

namespace WebApi.Messages;

public record ProjectCreatedMessage( Guid Id, string Title, string Acronym, PeriodDate periodDate);