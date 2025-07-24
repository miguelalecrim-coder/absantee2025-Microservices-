using Application.Interfaces;
using MassTransit;
using WebApi.Messages;

public class ProjectCreatedConsumer : IConsumer<ProjectCreatedMessage>
{
    private readonly IProjectService _projectService;


    public ProjectCreatedConsumer(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedMessage> context)
    {
        var msg = context.Message;
        await _projectService.SubmitAsync(msg.Id, msg.Title, msg.Acronym, msg.periodDate);
    }
}