using Application.Tests.ProjectServiceTests;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Moq;

public class ProjectServiceSubmiteAsync : ProjectServiceTestBase
{
   [Fact]
public async Task Submit_ShouldCallAddAndSave()
{
    // Arrange
    var id = Guid.NewGuid();
    var visitor = new ProjectDataModel
    {
        Id = id,
        Title = "Title",
        Acronym = "ACR",
        PeriodDate = new PeriodDate()
    };

    var project = new Project(id, visitor.Title, visitor.Acronym, visitor.PeriodDate);

    ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);

    // Act
    await ProjectService.SubmitAsync(id, visitor.Title, visitor.Acronym, visitor.PeriodDate);

    // Assert
    ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
    ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Once);
}


// [Fact]
// public async Task Submit_ShouldDoNothing_WhenProjectAlreadyExists()
// {
//     // Arrange
//     var id = Guid.NewGuid();
//     var title = "Angular";
//     var acronym = "A";
//     var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);

//     var existingProject = new Project(id, "Existing", "EX", periodDate);

//     ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(existingProject);

//     // Act
//     await ProjectService.SubmitAsync(id, title, acronym, periodDate);

//     // Assert
//     ProjectRepositoryDouble.Verify(x => x.GetByIdAsync(id), Times.Once);

    
//     ProjectFactoryDouble.Verify(x => x.Create(It.IsAny<ProjectDataModel>()), Times.Never);
//     ProjectRepositoryDouble.Verify(x => x.AddAsync(It.IsAny<Project>()), Times.Never);
//     ProjectRepositoryDouble.Verify(x => x.SaveChangesAsync(), Times.Never);
// }

}