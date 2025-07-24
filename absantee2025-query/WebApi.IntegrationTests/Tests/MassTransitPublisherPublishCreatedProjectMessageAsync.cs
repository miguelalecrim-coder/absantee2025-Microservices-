



using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Messages;
using Xunit;

namespace WebApi.IntegrationTests.MassTransitPublisherTests;

public class MassTransitPublisherPublishCreatedProjectMessageAsyncTests
{

    [Fact]
    public async Task WhenPublisherIsCalled_ThenPublishProject()
    {
        //arrange
        var endpointMock = new Mock<IPublishEndpoint>();
        var publisher = new MassTransitPublisher(endpointMock.Object);

        var projectId = Guid.NewGuid();

        //Act

        await publisher.PublishCreatedProjectMessageAsync(projectId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PeriodDate>());

        //Assert

        endpointMock.Verify(p => p.Publish(
            It.Is<ProjectCreatedMessage>(m =>
            m.Id == projectId),
             It.IsAny<CancellationToken>()),
            Times.Once

        );
    }
}