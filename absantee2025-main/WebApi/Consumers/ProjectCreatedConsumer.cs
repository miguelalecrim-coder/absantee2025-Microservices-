using MassTransit;
using WebApi.Messages;

public class ProjectCreatedConsumer : IConsumer<ProjectCreatedMessage>
{
    private readonly ProjectService _projectService;


    public ProjectCreatedConsumer(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedMessage> context)
    {
        var msg = context.Message;
        await _projectService.SubmitAsync(msg.id, msg.title, msg.acronym, msg.periodDate);
    }
}