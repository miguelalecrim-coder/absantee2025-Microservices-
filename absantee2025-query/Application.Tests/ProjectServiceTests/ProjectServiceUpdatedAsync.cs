


using Application.Tests.ProjectServiceTests;
using Domain.Models;
using Moq;

public class ProjectServiceUpdatedAsync : ProjectServiceTestBase
{

    [Fact]
    public async Task Updated_ShouldSaveResult()
    {
        //Arrange

        var id = Guid.NewGuid();
        var title = "Updated Title";
        var acronym = "UPD";
        var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);

        var existingProject = new Project(id, "Old Title", "OLD", periodDate);



        ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(existingProject);

        ProjectRepositoryDouble.Setup(x => x.UpdateProject(existingProject)).ReturnsAsync(existingProject);

        ProjectRepositoryDouble.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //Act

        await ProjectService.SubmitUpdatedAsync(id, title, acronym, periodDate);

        //Assert

        ProjectRepositoryDouble.Verify(x => x.GetByIdAsync(id), Times.Once);
        ProjectRepositoryDouble.Verify(x => x.UpdateProject(It.Is<Project>(p =>
            p.Id == id &&
            p.Title == title &&
            p.Acronym == acronym &&
            p.PeriodDate == periodDate
        )), Times.Once);

        ProjectRepositoryDouble.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]

public async Task SubmitUpdated_ShouldDoNothing_WhenProjectDoesNotExist()
{
    // Arrange
    var id = Guid.NewGuid();
    ProjectRepositoryDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Project)null);

    // Act
    await ProjectService.SubmitUpdatedAsync(id, "Title", "ACR", new PeriodDate());

    // Assert
    ProjectRepositoryDouble.Verify(r => r.GetByIdAsync(id), Times.Once);
    ProjectRepositoryDouble.Verify(r => r.UpdateProject(It.IsAny<Project>()), Times.Never);
    ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Never);
}



}