using AutoMapper;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using WebApi.Tests.Helpers;

namespace Infrastructure.Tests.ProjectRepositoryTests;

public class ProjectRepositoryGetByIdTests : RepositoryTestBase
{
    [Fact]
    public void GetById_ProjectExists_ReturnsProject()
    {
        var id = Guid.NewGuid();
        var projectDM = new ProjectDataModel
        {
            Id = id,
            Title = "Projeto Teste",
            Acronym = "PRJ001"
        };

        var context = DbContextMock.CreateWithData(projectDM);
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<ProjectDataModel, Project>(projectDM))
                  .Returns(new Project(id, "Projeto Teste", "PRJ001", 
                      new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(30)))));

        var repo = new ProjectRepositoryEF(context, mapperMock.Object);

        var result = repo.GetById(id);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetById_ProjectDoesNotExist_ReturnsNull()
    {
        var context = DbContextMock.CreateWithData();
        var mapperMock = new Mock<IMapper>();
        var repo = new ProjectRepositoryEF(context, mapperMock.Object);

        var result = repo.GetById(Guid.NewGuid());

        Assert.Null(result);
    }
}
