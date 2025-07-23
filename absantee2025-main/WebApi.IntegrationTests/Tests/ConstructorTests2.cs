
using Moq;
using Xunit;

public class ConstructorTest
{
    [Fact]
    public void WhenConstructorIsCalled_ThenObjectIsInstantiated()
    {
        // Arrange
        var serviceMock = new Mock<ProjectService>();

        // Act
        var consumer = new ProjectCreatedConsumer(serviceMock.Object);

        // Assert
    }
}