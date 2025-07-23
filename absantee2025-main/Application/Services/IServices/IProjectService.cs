using Application.DTO;
using Domain.Models;

namespace Application
{
    public interface IProjectService
    {
        Task<Result<ProjectDTO>> Add(CreateProjectDTO projectDTO);
        Task<Result<ProjectDTO>> EditProject(ProjectDTO projectDTO);
        Task SubmitAsync(Guid id, string title, string acronym, PeriodDate periodDate);
        Task SubmitUpdatedAsync(Guid id, string title, string acronym, PeriodDate periodDate);
    }
}
