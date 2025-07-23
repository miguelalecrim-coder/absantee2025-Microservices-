using Application.DTO;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<Result<IEnumerable<ProjectDTO>>> GetAll();
        Task<Result<ProjectDTO>> GetProjectById(Guid id);
        Task SubmitAsync(Guid id, string title, string acronym, PeriodDate periodDate);
        Task SubmitUpdatedAsync(Guid id, string title, string acronym, PeriodDate periodDate);
    }
}
