using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Infrastructure.Tests.ProjectRepositoryTests
{
    public class ProjectRepositoryUpdateProjectTests : RepositoryTestBase
    {
        [Fact]
        public async Task WhenProjectExists_ThenUpdatesAndReturnsProject()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var oldPeriodDate = new PeriodDate(
                DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
                DateOnly.FromDateTime(DateTime.Today)
            );

            var existingProjectDM = new ProjectDataModel
            {
                Id = guid,
                Title = "Old Title",
                Acronym = "OLD",
                PeriodDate = oldPeriodDate,
            };

            context.Projects.Add(existingProjectDM);
            await context.SaveChangesAsync();

            var newPeriodDate = new PeriodDate(
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today).AddDays(1)
            );

            var updatedProjectMock = new Mock<IProject>();
            updatedProjectMock.Setup(p => p.Id).Returns(guid);
            updatedProjectMock.Setup(p => p.Title).Returns("New Title");
            updatedProjectMock.Setup(p => p.Acronym).Returns("NEW");
            updatedProjectMock.Setup(p => p.PeriodDate).Returns(newPeriodDate);

            _mapper.Setup(m => m.Map<ProjectDataModel, Domain.Models.Project>(It.IsAny<ProjectDataModel>()))
                .Returns<ProjectDataModel>(dm => new Domain.Models.Project(dm.Id, dm.Title, dm.Acronym, dm.PeriodDate));

            var repo = new ProjectRepositoryEF(context, _mapper.Object);

            // Act
            var result = await repo.UpdateProject(updatedProjectMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("NEW", result.Acronym);
            Assert.Equal(newPeriodDate, result.PeriodDate);

            // Verifica direto no banco que foi atualizado
            var projectInDb = await context.Projects.FindAsync(guid);
            Assert.NotNull(projectInDb);
            Assert.Equal("New Title", projectInDb.Title);
            Assert.Equal("NEW", projectInDb.Acronym);
            Assert.Equal(newPeriodDate, projectInDb.PeriodDate);
        }

        [Fact]
        public async Task WhenProjectDoesNotExist_ThenReturnsNull()
        {
            var updatedProjectMock = new Mock<IProject>();
            updatedProjectMock.Setup(p => p.Id).Returns(Guid.NewGuid());

            var repo = new ProjectRepositoryEF(context, _mapper.Object);

            var result = await repo.UpdateProject(updatedProjectMock.Object);

            Assert.Null(result);
        }
    }
}
