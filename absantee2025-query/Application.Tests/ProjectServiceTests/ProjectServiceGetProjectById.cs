using Application.DTO;
using Domain.Models;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Tests.ProjectServiceTests;
using Moq;

namespace Application.Tests.ProjectTests;

public class ProjectServiceGetById : ProjectServiceTestBase
{
    [Fact]
    public async Task GetProjectById_ShouldReturnMappedDTO_WhenProjectExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project = new Project(id, "Project A", "PROJA", new PeriodDate());

        ProjectRepositoryDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(project);

        var expectedDto = new ProjectDTO(project.Id, project.Title, project.Acronym, project.PeriodDate);
        MapperDouble.Setup(m => m.Map<ProjectDTO>(project)).Returns(expectedDto);

        // Act
        var result = await ProjectService.GetProjectById(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(expectedDto.Id, result.Value.Id);
        Assert.Equal(expectedDto.Title, result.Value.Title);
        Assert.Equal(expectedDto.Acronym, result.Value.Acronym);
    }

    [Fact]
    public async Task GetProjectById_ShouldReturnFailure_WhenProjectDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        ProjectRepositoryDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Project)null);

        // Act
        var result = await ProjectService.GetProjectById(id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found", result.Error.Message);
    }

    [Fact]
    public async Task GetProjectById_ShouldCallRepositoryOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        ProjectRepositoryDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Project)null);

        // Act
        await ProjectService.GetProjectById(id);

        // Assert
        ProjectRepositoryDouble.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetProjectById_ShouldMapProject_WhenProjectExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project = new Project(id, "Test", "TEST", new PeriodDate());

        ProjectRepositoryDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(project);
        MapperDouble.Setup(m => m.Map<ProjectDTO>(project)).Returns(new ProjectDTO(id, project.Title, project.Acronym, project.PeriodDate));

        // Act
        var result = await ProjectService.GetProjectById(id);

        // Assert
        MapperDouble.Verify(m => m.Map<ProjectDTO>(project), Times.Once);
    }
}
