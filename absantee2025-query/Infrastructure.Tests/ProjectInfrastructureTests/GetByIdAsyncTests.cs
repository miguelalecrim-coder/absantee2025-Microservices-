using AutoMapper;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using WebApi.Tests.Helpers;

namespace Infrastructure.Tests.ProjectRepositoryTests;

public class ProjectRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    private Project CreateValidProject(Guid id)
    {
        return new Project(
            id,
            "Projeto Teste",
            "PRJ123",
            new PeriodDate(
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddDays(30))
            )
        );
    }

    [Fact]
    public async Task GetByIdAsync_ProjectExists_ReturnsProject()
    {
        var id = Guid.NewGuid();
        var projectDM = new ProjectDataModel
        {
            Id = id,
            Acronym = "PRJ",
            Title = "Teste"
        };

        var context = DbContextMock.CreateWithData(projectDM);
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<ProjectDataModel, Project>(projectDM))
                  .Returns(CreateValidProject(id));

        var repo = new ProjectRepositoryEF(context, mapperMock.Object);

        var result = await repo.GetByIdAsync(id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ProjectNotFound_ReturnsNull()
    {
        var context = DbContextMock.CreateWithData();
        var mapperMock = new Mock<IMapper>();
        var repo = new ProjectRepositoryEF(context, mapperMock.Object);

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_MultipleIds_ReturnsProjects()
    {
        var project1 = new ProjectDataModel
        {
            Id = Guid.NewGuid(),
            Title = "Project 1",
            Acronym = "P1"
        };

        var project2 = new ProjectDataModel
        {
            Id = Guid.NewGuid(),
            Title = "Project 2",
            Acronym = "P2"
        };

        var context = DbContextMock.CreateWithData(project1, project2);
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<ProjectDataModel, Project>(It.IsAny<ProjectDataModel>()))
                  .Returns<ProjectDataModel>(p => CreateValidProject(p.Id));

        var repo = new ProjectRepositoryEF(context, mapperMock.Object);

        var result = await repo.GetByIdAsync(new[] { project1.Id, project2.Id });

        Assert.Equal(2, result.Count());
    }
}

