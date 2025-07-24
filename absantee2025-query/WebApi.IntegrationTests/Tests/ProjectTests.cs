using Application;
using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using Xunit;
using Moq;
using Domain.Models;

public class ProjectControllerGetByIdTests
{
    [Fact]
    public async Task WhenGetByIdIsCalled_WithValidId_ThenReturnsOkWithProject()
    {
        // Arrange
        var mockService = new Mock<IProjectService>();
        var id = Guid.NewGuid();
        var project = new ProjectDTO { Id = id, Title = "Test", Acronym = "TST" };

        mockService.Setup(s => s.GetProjectById(id))
                   .ReturnsAsync(Result<ProjectDTO>.Success(project));
        var controller = new ProjectController(mockService.Object);

        // Act
        var result = await controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<ProjectDTO>(okResult.Value);
        Assert.Equal(id, returned.Id);
    }

    [Fact]
    public async Task WhenGetByIdIsCalled_WithInvalidId_ThenReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<IProjectService>();
        var id = Guid.NewGuid();

        mockService.Setup(s => s.GetProjectById(id))
                   .ReturnsAsync(Result<ProjectDTO>.Failure(Error.NotFound("Project not found")));

        var controller = new ProjectController(mockService.Object);

        // Act
        var result = await controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

      [Fact]
    public async Task WhenGetAllIsCalled_WithExistingProjects_ThenReturnsOkWithProjects()
    {
        // Arrange
        var mockService = new Mock<IProjectService>();
        var projects = new List<ProjectDTO>
        {
            new ProjectDTO { 
                Id = Guid.NewGuid(), 
                Title = "Project 1", 
                Acronym = "P1", 
                PeriodDate = new PeriodDate(new DateOnly(2025, 1, 1), new DateOnly(2025, 6, 30)) 
            },
            new ProjectDTO { 
                Id = Guid.NewGuid(), 
                Title = "Project 2", 
                Acronym = "P2", 
                PeriodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 12, 31)) 
            }
        };

        mockService.Setup(s => s.GetAll())
                   .ReturnsAsync(Result<IEnumerable<ProjectDTO>>.Success(projects));

        var controller = new ProjectController(mockService.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProjects = Assert.IsAssignableFrom<IEnumerable<ProjectDTO>>(okResult.Value);

        Assert.Equal(projects.Count, returnedProjects.Count());
        Assert.Contains(returnedProjects, p => p.Title == "Project 1" && p.PeriodDate.InitDate.Year == 2025);
        Assert.Contains(returnedProjects, p => p.Title == "Project 2" && p.PeriodDate.FinalDate.Month == 12);
    }

    [Fact]
    public async Task WhenGetAllIsCalled_WithNoProjects_ThenReturnsOkWithEmptyList()
    {
        // Arrange
        var mockService = new Mock<IProjectService>();
        var emptyProjects = new List<ProjectDTO>();

        mockService.Setup(s => s.GetAll())
                   .ReturnsAsync(Result<IEnumerable<ProjectDTO>>.Success(emptyProjects));

        var controller = new ProjectController(mockService.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProjects = Assert.IsAssignableFrom<IEnumerable<ProjectDTO>>(okResult.Value);
        Assert.Empty(returnedProjects);
    }

}
