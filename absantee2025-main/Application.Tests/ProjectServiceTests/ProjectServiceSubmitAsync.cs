using Application.Tests.ProjectServiceTests;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Moq;

public class ProjectServiceSubmiteAsync : ProjectServiceTestBase
{
    [Fact]
    public async Task Submit_ShouldSaveResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Angular";
        var acronym = "A";
        var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);

        ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((IProject)null);

        var expectedProject = new Project(id, title, acronym, periodDate);

        ProjectFactoryDouble.Setup(x => x.Create(It.Is<ProjectDataModel>(
            d => d.Id == id &&
                 d.Title == title &&
                 d.Acronym == acronym &&
                 d.PeriodDate == periodDate
        ))).Returns(expectedProject);

        ProjectRepositoryDouble.Setup(x => x.AddAsync(expectedProject)).ReturnsAsync(expectedProject);
        ProjectRepositoryDouble.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id, title, acronym, periodDate);

        // Assert
        ProjectRepositoryDouble.Verify(x => x.GetByIdAsync(id), Times.Once);
        ProjectFactoryDouble.Verify(x => x.Create(It.IsAny<ProjectDataModel>()), Times.Once);
        ProjectRepositoryDouble.Verify(x => x.AddAsync(expectedProject), Times.Once);
        ProjectRepositoryDouble.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

[Fact]
public async Task Submit_ShouldDoNothing_WhenProjectAlreadyExists()
{
    // Arrange
    var id = Guid.NewGuid();
    var title = "Angular";
    var acronym = "A";
    var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);

    var existingProject = new Project(id, "Existing", "EX", periodDate);

    ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(existingProject);

    // Act
    await ProjectService.SubmitAsync(id, title, acronym, periodDate);

    // Assert
    ProjectRepositoryDouble.Verify(x => x.GetByIdAsync(id), Times.Once);

    
    ProjectFactoryDouble.Verify(x => x.Create(It.IsAny<ProjectDataModel>()), Times.Never);
    ProjectRepositoryDouble.Verify(x => x.AddAsync(It.IsAny<Project>()), Times.Never);
    ProjectRepositoryDouble.Verify(x => x.SaveChangesAsync(), Times.Never);
}



}