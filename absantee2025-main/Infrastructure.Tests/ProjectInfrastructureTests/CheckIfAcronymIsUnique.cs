using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Infrastructure.Tests.ProjectRepositoryTests
{
    public class ProjectRepositoryCheckIfAcronymIsUniqueTests : RepositoryTestBase
    {
        [Fact]
        public async Task WhenAcronymDoesNotExist_ReturnsTrue()
        {
            var repo = new ProjectRepositoryEF(context, _mapper.Object);

            var result = await repo.CheckIfAcronymIsUnique("UNIQUE_ACRONYM");

            Assert.True(result);
        }

        [Fact]
        public async Task WhenAcronymExists_ReturnsFalse()
        {
            var acronym = "EXISTING";
            var periodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.Today).AddDays(-1), DateOnly.FromDateTime(DateTime.Today));
            var project = new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Acronym = acronym,
                Title = "Test Project",
                PeriodDate = periodDate,
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var repo = new ProjectRepositoryEF(context, _mapper.Object);

            var result = await repo.CheckIfAcronymIsUnique(acronym);

            Assert.False(result);
        }
    }
}
