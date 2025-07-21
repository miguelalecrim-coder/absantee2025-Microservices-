
using MassTransit;
using MassTransit.Middleware;
using WebApi.Messages;


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
    await _projectService.SubmitUpdatedAsync(msg.Id, msg.Title, msg.Acronym, msg.periodDate);
}
}