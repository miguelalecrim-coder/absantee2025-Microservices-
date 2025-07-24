using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using Domain.Models;
using WebApi.Tests.Helpers;
using AutoMapper;

namespace Infrastructure.Tests.ProjectRepositoryTests
{
    public class ProjectRepositoryUpdateProjectTests : RepositoryTestBase
    {
        [Fact]
        public async Task UpdateProject_ExistingProject_UpdatesAndReturnsMapped()
        {
            var id = Guid.NewGuid();
            var existing = new ProjectDataModel
            {
                Id = id,
                Title = "Original",
                Acronym = "OLD"
            };

            var updated = new Project(id, "Updated", "NEW", 
                new PeriodDate(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(1))));

            var context = DbContextMock.CreateWithData(existing);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ProjectDataModel, Project>(It.IsAny<ProjectDataModel>()))
                      .Returns(updated);

            var repo = new ProjectRepositoryEF(context, mapperMock.Object);

            var result = await repo.UpdateProject(updated);

            Assert.NotNull(result);
            Assert.Equal("Updated", result.Title);
            Assert.Equal("NEW", result.Acronym);
        }

        [Fact]
        public async Task UpdateProject_ProjectDoesNotExist_ReturnsNull()
        {
            var updated = new Project(Guid.NewGuid(), "New Project", "ACR123",
                new PeriodDate(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(10))));

            var context = DbContextMock.CreateWithData(); // empty
            var mapperMock = new Mock<IMapper>();
            var repo = new ProjectRepositoryEF(context, mapperMock.Object);

            var result = await repo.UpdateProject(updated);

            Assert.Null(result);
        }
    }
}
