using Xunit;
using MassTransit;
using Moq;


namespace WebApi.IntegrationTests.MassTransitPublisherTests;


public class ConstructorTest
{
    [Fact]
    public void WhenConstructorIsCalled_ThenObjectIsInstantiated()
    {
        //Arrange 
        var endpointMock = new Mock<IPublishEndpoint>();

        //Act
        var publisher = new MassTransitPublisher(endpointMock.Object);
    }

}