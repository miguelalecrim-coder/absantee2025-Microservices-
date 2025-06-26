
using Application.DTO;
using Domain.Models;
using Moq;
namespace Application.Tests.ProjectServiceTests;


public class ProjectServiceAdd : ProjectServiceTestBase
{
    [Fact]
    public async Task Create_ShouldReturnSuccessResult()
    {
        //Arrange

        var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);

        var createDTO = new CreateProjectDTO("angular", "A", periodDate);

        var project = new Project(Guid.NewGuid(), createDTO.Title, createDTO.Acronym, createDTO.PeriodDate);
        var projectDTO = new ProjectDTO(project.Id, project.Title, project.Acronym, project.PeriodDate);

        ProjectFactoryDouble.Setup(x => x.Create(createDTO.Title, createDTO.Acronym, createDTO.PeriodDate)).ReturnsAsync(project);

        ProjectRepositoryDouble.Setup(x => x.AddAsync(project)).ReturnsAsync(project);

        PublisherDouble.Setup(p => p.PublishCreatedProjectMessageAsync(project.Id, project.Title, project.Acronym, project.PeriodDate)).Returns(Task.CompletedTask);

        MapperDouble.Setup(m => m.Map<Project, ProjectDTO>(project)).Returns(projectDTO);

        //Act
        var result = await ProjectService.Add(createDTO);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(projectDTO.Id, result.Value.Id);
        Assert.Equal(projectDTO.Title, result.Value.Title);
        Assert.Equal(projectDTO.Acronym, result.Value.Acronym);
        Assert.Equal(projectDTO.PeriodDate, result.Value.PeriodDate);

        ProjectFactoryDouble.Verify(f => f.Create(createDTO.Title, createDTO.Acronym, createDTO.PeriodDate), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
        PublisherDouble.Verify(p => p.PublishCreatedProjectMessageAsync(project.Id, project.Title, project.Acronym, project.PeriodDate), Times.Once);
        MapperDouble.Verify(m => m.Map<Project, ProjectDTO>(project), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldReturnErrorResult()
    {
        //Arrange

        var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);
        var createDTO = new CreateProjectDTO("angular", "A", periodDate);

        ProjectFactoryDouble.Setup(x => x.Create(createDTO.Title, createDTO.Acronym, createDTO.PeriodDate)).ThrowsAsync(new ArgumentException("Project should be unique"));

        //Act

        var result = await ProjectService.Add(createDTO);

        //Assert

        Assert.False(result.IsSuccess);
        Assert.Equal("Project should be unique", result.Error.Message);
    }

    [Fact]
    public async Task Create_ShoulReturnInternalServerError_WhenUnexpectedExceptionsOccurs()
    {
        //Arrange

        var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);
        var createDTO = new CreateProjectDTO("angular", "A", periodDate);

        ProjectFactoryDouble.Setup(x => x.Create(createDTO.Title, createDTO.Acronym, createDTO.PeriodDate)).ThrowsAsync(new InvalidOperationException("Unexpected failure"));

        //Act
        var result = await ProjectService.Add(createDTO);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Unexpected failure", result.Error.Message);
    }

}

