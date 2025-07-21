using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;




namespace Infrastructure.Tests.ProjectRepositoryTests;

public class ProjectRepositoryGetByIdTests : RepositoryTestBase
{
  public async Task WhenSearchingById_ThenReturnsProject()
  {
    //arrange

    var project1 = new Mock<Project>();
    var guid1 = Guid.NewGuid();
    project1.Setup(p => p.Id).Returns(guid1);
    var projectDM1 = new ProjectDataModel(project1.Object);
    context.Projects.Add(projectDM1);

    var project2 = new Mock<Project>();
    var guid2 = Guid.NewGuid();
    project2.Setup(p => p.Id).Returns(guid2);
    var projectDM2 = new ProjectDataModel(project2.Object);
    context.Projects.Add(projectDM2);

    await context.SaveChangesAsync();

    var expected = new Mock<Project>().Object;

    _mapper.Setup(m => m.Map<ProjectDataModel, Project>(
    It.Is<ProjectDataModel>(t => t.Id == projectDM1.Id)
  )).Returns(new Project(projectDM1.Id, projectDM1.Title, projectDM1.Acronym, projectDM1.PeriodDate));


    var projectRepository = new ProjectRepositoryEF(context, _mapper.Object);

    //act

    var result = projectRepository.GetById(guid1);


    //assert

    Assert.NotNull(result);
    Assert.Equal(projectDM1.Id, result.Id);
  }

  [Fact]
  public async Task WhenSearchingByIdWithNoProjects_ThenReturnsNull()
  {
    var project1 = new Mock<Project>();
    var guid1 = Guid.NewGuid();
    var guid2 = Guid.NewGuid();
    project1.Setup(c => c.Id).Returns(guid1);
    var projectDM1 = new ProjectDataModel(project1.Object);
    context.Projects.Add(projectDM1);

    await context.SaveChangesAsync();

    _mapper.Setup(p => p.Map<ProjectDataModel, Project>(
      It.Is<ProjectDataModel>(t => t.Id == projectDM1.Id)
    )).Returns(new Project(projectDM1.Id, projectDM1.Title, projectDM1.Acronym, projectDM1.PeriodDate));

    var projectRepository = new ProjectRepositoryEF(context, _mapper.Object);

    var result = projectRepository.GetById(guid2);

    Assert.Null(result);
  }
}