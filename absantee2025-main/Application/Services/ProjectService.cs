using Application;
using Application.DTO;
using Application.Messaging;

using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;


public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IProjectFactory _factory;
    private readonly IMessagePublisher _publisher;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository repository, IProjectFactory factory, IMessagePublisher messagePublisher, IMapper mapper)
    {
        _repository = repository;
        _factory = factory;
        _publisher = messagePublisher;
        _mapper = mapper;
    }

    public ProjectService()
    {
       
    }

    public async Task<Result<ProjectDTO>> Add(CreateProjectDTO projectDTO)
    {
        try
        {
            var proj = await _factory.Create(projectDTO.Title, projectDTO.Acronym, projectDTO.PeriodDate);
            await _repository.AddAsync(proj);

            await _publisher.PublishCreatedProjectMessageAsync(proj.Id, proj.Title, proj.Acronym, proj.PeriodDate);

            var projectDto = _mapper.Map<Project, ProjectDTO>(proj);
            return Result<ProjectDTO>.Success(projectDto);
        }
        catch (ArgumentException a)
        {
            return Result<ProjectDTO>.Failure(Error.BadRequest(a.Message));
        }
        catch (Exception ex)
        {
            return Result<ProjectDTO>.Failure(Error.InternalServerError(ex.Message));
        }
    }


    public async Task<Result<ProjectDTO>> EditProject(ProjectDTO projectDTO)
    {
        var project = await _repository.GetByIdAsync(projectDTO.Id);
        if (project == null)
            return Result<ProjectDTO>.Failure(Error.NotFound("Project not found"));

        project.UpdateTitle(projectDTO.Title);
        project.UpdateAcronym(projectDTO.Acronym);
        project.UpdatePeriodDate(projectDTO.PeriodDate);

        var updatedProject = await _repository.UpdateProject(project);

        if (updatedProject == null)
            return Result<ProjectDTO>.Failure(Error.InternalServerError("Failed to update project"));

        // Publish update message
        await _publisher.PublishUpdatedProjectMessageAsync(
            updatedProject.Id,
            updatedProject.Title,
            updatedProject.Acronym,
            updatedProject.PeriodDate
        );


        var updatedDto = _mapper.Map<ProjectDTO>(updatedProject);
        return Result<ProjectDTO>.Success(updatedDto);
    }

    public async Task SubmitAsync(Guid id, string title, string acronym, PeriodDate periodDate)
    {

        var existing = await _repository.GetByIdAsync(id);

        if (existing != null)
        {
            return;
        }

        var visitor = new ProjectDataModel()
        {
            Id = id,
            Title = title,
            Acronym = acronym,
            PeriodDate = periodDate
        };

        var proj = _factory.Create(visitor);

        await _repository.AddAsync(proj);
        await _repository.SaveChangesAsync();
    }

    public async Task SubmitUpdatedAsync(Guid id, string title, string acronym, PeriodDate periodDate)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null)
            return;

        project.UpdateTitle(title);
        project.UpdateAcronym(acronym);
        project.UpdatePeriodDate(periodDate);

        await _repository.UpdateProject(project);
        await _repository.SaveChangesAsync();
    }
}
