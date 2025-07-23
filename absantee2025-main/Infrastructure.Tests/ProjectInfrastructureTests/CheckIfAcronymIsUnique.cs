using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using WebApi.Tests.Helpers;
using AutoMapper;

namespace Infrastructure.Tests.ProjectRepositoryTests
{
    public class ProjectRepositoryCheckIfAcronymIsUniqueTests : RepositoryTestBase
    {
        [Fact]
        public async Task CheckIfAcronymIsUnique_Unique_ReturnsTrue()
        {
            // Arrange
            var context = DbContextMock.CreateWithData(); // vazio
            var mapperMock = new Mock<IMapper>();
            var repo = new ProjectRepositoryEF(context, mapperMock.Object);

            // Act
            var result = await repo.CheckIfAcronymIsUnique("NEW123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfAcronymIsUnique_Exists_ReturnsFalse()
        {
            // Arrange
            var project = new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Acronym = "DUPLICATE",
                Title = "Test Project"
            };

            var context = DbContextMock.CreateWithData(project);
            var mapperMock = new Mock<IMapper>();
            var repo = new ProjectRepositoryEF(context, mapperMock.Object);

            // Act
            var result = await repo.CheckIfAcronymIsUnique("DUPLICATE");

            // Assert
            Assert.False(result);
        }
    }
}
