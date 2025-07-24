using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Messages;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.IntegrationTests.MassTransitPublisherTests
{
    public class MassTransitPublisherPublishUpdateProjectMessageAsyncTests
    {

        [Fact]
    public async Task WhenPublisherIsCalled_ThenPublishProjectUpdate()
    {
        // Arrange
        var endpointMock = new Mock<IPublishEndpoint>();
        var publisher = new MassTransitPublisher(endpointMock.Object);

        var projectId = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var initDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var finalDate = initDate.AddDays(30);

        var period = new PeriodDate
        {
            InitDate = initDate,
            FinalDate = finalDate
        };

        // Act
        await publisher.PublishUpdatedProjectMessageAsync(projectId, title, acronym, period);

        // Assert
        endpointMock.Verify(p => p.Publish(
            It.Is<ProjectUpdatedMessage>(m =>
                m.Id == projectId &&
                m.Title == title &&
                m.Acronym == acronym &&
                m.periodDate.InitDate == initDate &&
                m.periodDate.FinalDate == finalDate),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

        }
}
