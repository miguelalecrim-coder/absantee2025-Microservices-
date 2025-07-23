using Application.DTO;
using Application.Tests.ProjectServiceTests;
using AutoMapper;
using Domain.Models;
using Moq;

namespace Application.Tests.ProjectTests;

public class ProjectServiceGetAll : ProjectServiceTestBase
{
    [Fact]
    public async Task GetAll_ShouldReturnMappedProjectDTOsInSuccessResult()
    {
        // Arrange
        var periodDateA = new PeriodDate
        {
            InitDate = new DateOnly(2025, 1, 1),
            FinalDate = new DateOnly(2025, 12, 31)
        };
        var periodDateB = new PeriodDate
        {
            InitDate = new DateOnly(2025, 2, 1),
            FinalDate = new DateOnly(2025, 11, 30)
        };

        var projects = new List<Project>
        {
            new("Project A", "PA", periodDateA),
            new("Project B", "PB", periodDateB)
        };

        ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

        MapperDouble.Setup(m => m.Map<ProjectDTO>(It.Is<Project>(p => p.Id == projects[0].Id)))
            .Returns(new ProjectDTO(projects[0].Id, projects[0].Title, projects[0].Acronym, projects[0].PeriodDate));

        MapperDouble.Setup(m => m.Map<ProjectDTO>(It.Is<Project>(p => p.Id == projects[1].Id)))
            .Returns(new ProjectDTO(projects[1].Id, projects[1].Title, projects[1].Acronym, projects[1].PeriodDate));

        // Act
        var result = await ProjectService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());

        Assert.Contains(result.Value, dto =>
            dto.Id == projects[0].Id &&
            dto.Title == projects[0].Title &&
            dto.Acronym == projects[0].Acronym &&
            dto.PeriodDate.Equals(projects[0].PeriodDate));

        Assert.Contains(result.Value, dto =>
            dto.Id == projects[1].Id &&
            dto.Title == projects[1].Title &&
            dto.Acronym == projects[1].Acronym &&
            dto.PeriodDate.Equals(projects[1].PeriodDate));
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Project>());

        // Act
        var result = await ProjectService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        ProjectRepositoryDouble
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => ProjectService.GetAll());
        Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task GetAll_ShouldCallRepositoryExactlyOnce()
    {
        // Arrange
        ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Project>());

        // Act
        await ProjectService.GetAll();

        // Assert
        ProjectRepositoryDouble.Verify(r => r.GetAllAsync(), Times.Once);
    }

//     [Fact]
//     public async Task GetAll_ShouldMapEachProjectExactlyOnce()
//     {
//         // Arrange
//         var projects = new List<Project>
//         {
//             new("P1", "P1", new PeriodDate()),
//             new("P2", "P2", new PeriodDate())
//         };

//         ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

//         foreach (var project in projects)
//         {
//             MapperDouble.Setup(m => m.Map<ProjectDTO>(project))
//                 .Returns(new ProjectDTO(project.Id, project.Title, project.Acronym, project.PeriodDate));
//         }

//         // Act
//         var result = await ProjectService.GetAll();

//         // Assert
//         MapperDouble.Verify(m => m.Map<ProjectDTO>(It.IsAny<Project>()), Times.Exactly(projects.Count));
//     }
 }
