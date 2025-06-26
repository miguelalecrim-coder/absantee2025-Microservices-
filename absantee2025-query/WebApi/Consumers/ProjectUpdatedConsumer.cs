using WebApi.Messages;
using MassTransit;
using MassTransit.Middleware;


public class ProjectUpdatedConsumer : IConsumer<ProjectUpdatedMessage>
{
    private readonly ProjectService _projectService;

    public ProjectUpdatedConsumer(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task Consume(ConsumeContext<ProjectUpdatedMessage> context)
{
    var msg = context.Message;
    await _projectService.SubmitUpdatedAsync(msg.id, msg.title, msg.acronym, msg.periodDate);
}
}