

using Application;
using Application.Interfaces;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Messages;
using Xunit;

namespace WebApi.IntegrationTests.ProjectCreatedConsumerTests;

public class ProjectCreatedConsumerConsumeTests
{
    [Fact]
    public async Task WhenMessageIsConsumed_ThenServiceMethodIsCalledWithData()
    {
        //Arrange

        var serviceMock = new Mock<IProjectService>();
        var consumer = new ProjectCreatedConsumer(serviceMock.Object);

        var projectId = new Guid();
        var title = "title";
        var acronym = "TTTT";
        var period = It.IsAny<PeriodDate>();

        var message = new ProjectCreatedMessage(projectId, title, acronym, period);

        var contextMock = Mock.Of<ConsumeContext<ProjectCreatedMessage>>(c => c.Message == message);
        //Act
        await consumer.Consume(contextMock);

        //Assert

        serviceMock.Verify(s => s.SubmitAsync(projectId,title,acronym,period));
    }
}