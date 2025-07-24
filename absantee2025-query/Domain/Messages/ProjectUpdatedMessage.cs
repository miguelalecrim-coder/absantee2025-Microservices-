
using Domain.Models;

namespace WebApi.Messages;

public record ProjectUpdatedMessage(Guid Id, string Title, string Acronym, PeriodDate periodDate);