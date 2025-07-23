using Application;
using Application.DTO;
using Application.Interfaces;
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

    private readonly IMapper _mapper;
    private readonly IMessagePublisher _publisher;

    public ProjectService(IProjectRepository repository, IProjectFactory factory, IMessagePublisher messagePublisher, IMapper mapper)
    {
        _repository = repository;
        _factory = factory;
        _publisher = messagePublisher;
        _mapper = mapper;
    }

    
    public async Task<Result<IEnumerable<ProjectDTO>>> GetAll()
    {
        var projects = await _repository.GetAllAsync();
        var result = projects.Select(_mapper.Map<ProjectDTO>);
        return Result<IEnumerable<ProjectDTO>>.Success(result);
    }

     public async Task<Result<ProjectDTO>> GetProjectById(Guid id)
        {
            var project = await _repository.GetByIdAsync(id);
            if (project == null)
                return Result<ProjectDTO>.Failure(Error.NotFound("Project not found"));
            var result = _mapper.Map<ProjectDTO>(project);
            return Result<ProjectDTO>.Success(result);
        }

    
    public async Task SubmitAsync(Guid id, string title, string acronym, PeriodDate periodDate)
    {
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
