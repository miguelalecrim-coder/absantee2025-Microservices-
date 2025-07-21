using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;




namespace Infrastructure.Tests.ProjectRepositoryTests;

public class ProjectRepositoryGetByIdAsyncTests : RepositoryTestBase
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

    var result = await projectRepository.GetByIdAsync(guid1);


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

    var result = await projectRepository.GetByIdAsync(guid2);

    Assert.Null(result);
  }

  [Fact]
        public async Task WhenProjectsExist_ReturnsProjects()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
          var periodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.Today).AddDays(-1), DateOnly.FromDateTime(DateTime.Today));


            var projectDM1 = new ProjectDataModel
            {
                Id = guid1,
                Title = "Project 1",
                Acronym = "P1",
                PeriodDate = periodDate,
            };

            var projectDM2 = new ProjectDataModel
            {
                Id = guid2,
                Title = "Project 2",
                Acronym = "P2",
                PeriodDate = periodDate,
            };

            context.Projects.AddRange(projectDM1, projectDM2);
            await context.SaveChangesAsync();

            _mapper.Setup(m => m.Map<ProjectDataModel, Project>(It.Is<ProjectDataModel>(p => p.Id == guid1)))
                   .Returns(new Project(projectDM1.Id, projectDM1.Title, projectDM1.Acronym, projectDM1.PeriodDate));
            
            _mapper.Setup(m => m.Map<ProjectDataModel, Project>(It.Is<ProjectDataModel>(p => p.Id == guid2)))
                   .Returns(new Project(projectDM2.Id, projectDM2.Title, projectDM2.Acronym, projectDM2.PeriodDate));

            var repo = new ProjectRepositoryEF(context, _mapper.Object);

            var results = await repo.GetByIdAsync([guid1, guid2]);

            Assert.NotNull(results);
            Assert.Equal(2, results.Count());
            Assert.Contains(results, r => r.Id == guid1);
            Assert.Contains(results, r => r.Id == guid2);
        }

        [Fact]
        public async Task WhenNoMatchingProjects_ReturnsEmptyCollection()
        {
            var repo = new ProjectRepositoryEF(context, _mapper.Object);

            var results = await repo.GetByIdAsync([Guid.NewGuid(), Guid.NewGuid()]);

            Assert.NotNull(results);
            Assert.Empty(results);
        }
}