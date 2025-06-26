using Moq;


namespace Application.Tests.ProjectTests;


public class ProjectServiceGetAll : ProjectServiceTestBase
{

    [Fact]
    public async Task GetAll_ShouldReturnMappedProjectDTOsInSuccessResult()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Project A",
                Acronym = "PA",
                PeriodDate = new PeriodDate
                {
                    InitDate = new DateOnly(2025, 1, 1),
                    FinalDate = new DateOnly(2025, 12, 31)
                }
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Project B",
                Acronym = "PB",
                PeriodDate = new PeriodDate
                {
                    InitDate = new DateOnly(2025, 2, 1),
                    FinalDate = new DateOnly(2025, 11, 30)
                }
            }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

        // Mock mapping for each Project to ProjectDTO
        _mapperMock.Setup(m => m.Map<ProjectDTO>(It.Is<Project>(p => p.Id == projects[0].Id)))
            .Returns(new ProjectDTO(projects[0].Id, projects[0].Title, projects[0].Acronym, projects[0].PeriodDate));

        _mapperMock.Setup(m => m.Map<ProjectDTO>(It.Is<Project>(p => p.Id == projects[1].Id)))
            .Returns(new ProjectDTO(projects[1].Id, projects[1].Title, projects[1].Acronym, projects[1].PeriodDate));

        // Act
        var result = await _service.GetAll();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);

        result.Value.Should().ContainEquivalentOf(new ProjectDTO(
            projects[0].Id, projects[0].Title, projects[0].Acronym, projects[0].PeriodDate));

        result.Value.Should().ContainEquivalentOf(new ProjectDTO(
            projects[1].Id, projects[1].Title, projects[1].Acronym, projects[1].PeriodDate));
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Project>());

        // Act
        var result = await _service.GetAll();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _service.GetAll();

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");


    }

    [Fact]
    public async Task GetAll_ShouldCallRepositoryExactlyOnce()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Project>());

        // Act
        await _service.GetAll();

        // Assert
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldMapEachProjectExactlyOnce()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), Title = "P1", Acronym = "P1", PeriodDate = new PeriodDate() },
            new Project { Id = Guid.NewGuid(), Title = "P2", Acronym = "P2", PeriodDate = new PeriodDate() }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

        foreach (var project in projects)
        {
            _mapperMock.Setup(m => m.Map<ProjectDTO>(project))
                .Returns(new ProjectDTO(project.Id, project.Title, project.Acronym, project.PeriodDate));
        }

        // Act
        var result = await _service.GetAll();

        // Assert
        _mapperMock.Verify(m => m.Map<ProjectDTO>(It.IsAny<Project>()), Times.Exactly(projects.Count));
    }



}