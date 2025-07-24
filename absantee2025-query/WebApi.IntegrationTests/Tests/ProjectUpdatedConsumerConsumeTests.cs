


using Application;
using Application.Interfaces;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Messages;
using Xunit;

namespace WebApi.IntegrationTests.ProjectUpdatedConsumerTests;

public class ProjectUpdatedConsumerConsumeTests
{
    [Fact]

    public async Task WhenMessageIsConsumed_ThenUpdateMethodIsCalledWithData()
    {
        // Arrange
        var serviceMock = new Mock<IProjectService>();
        var consumer = new ProjectUpdatedConsumer(serviceMock.Object);

        var projectId = Guid.NewGuid();
        var title = "Updated Title";
        var acronym = "UPDT";
        var period = It.IsAny<PeriodDate>();

        var message = new ProjectUpdatedMessage(projectId, title, acronym, period);

        var contextMock = Mock.Of<ConsumeContext<ProjectUpdatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(contextMock);

        // Assert
        serviceMock.Verify(s => s.SubmitUpdatedAsync(projectId, title, acronym, period), Times.Once);
    }
}