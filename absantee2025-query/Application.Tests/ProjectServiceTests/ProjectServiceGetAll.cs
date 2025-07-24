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

  
    [Fact]
    public async Task GetAll_ShouldReturnProjectsInSameOrderAsRepository()
    {
        // Arrange
        var projects = new List<Project>
        {
            new("Project Z", "PZ", new PeriodDate { InitDate = new DateOnly(2025, 1, 1), FinalDate = new DateOnly(2025, 12, 31) }),
            new("Project A", "PA", new PeriodDate { InitDate = new DateOnly(2025, 2, 1), FinalDate = new DateOnly(2025, 11, 30) }),
            new("Project M", "PM", new PeriodDate { InitDate = new DateOnly(2025, 3, 1), FinalDate = new DateOnly(2025, 10, 31) })
        };

        ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

        MapperDouble.Setup(m => m.Map<ProjectDTO>(It.IsAny<Project>()))
            .Returns((Project p) => new ProjectDTO(p.Id, p.Title, p.Acronym, p.PeriodDate));

        // Act
        var result = await ProjectService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        var resultList = result.Value.ToList();
        
        Assert.Equal(projects[0].Id, resultList[0].Id);
        Assert.Equal(projects[1].Id, resultList[1].Id);
        Assert.Equal(projects[2].Id, resultList[2].Id);
        
        Assert.Equal("Project Z", resultList[0].Title);
        Assert.Equal("Project A", resultList[1].Title);
        Assert.Equal("Project M", resultList[2].Title);
    }

    [Fact]
    public async Task GetAll_ShouldHandleLargeNumberOfProjects()
    {
        // Arrange
        const int projectCount = 1000;
        var projects = new List<Project>();
        
        for (int i = 0; i < projectCount; i++)
        {
            projects.Add(new Project($"Project {i}", $"P{i}", new PeriodDate 
            { 
                InitDate = new DateOnly(2025, 1, 1), 
                FinalDate = new DateOnly(2025, 12, 31) 
            }));
        }

        ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);
        MapperDouble.Setup(m => m.Map<ProjectDTO>(It.IsAny<Project>()))
            .Returns((Project p) => new ProjectDTO(p.Id, p.Title, p.Acronym, p.PeriodDate));

        // Act
        var result = await ProjectService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projectCount, result.Value.Count());
        MapperDouble.Verify(m => m.Map<ProjectDTO>(It.IsAny<Project>()), Times.Exactly(projectCount));
    }

    [Fact]
    public async Task GetAll_ShouldReturnDifferentInstancesOnMultipleCalls()
    {
        // Arrange
        var project = new Project("Test Project", "TP", new PeriodDate 
        { 
            InitDate = new DateOnly(2025, 1, 1), 
            FinalDate = new DateOnly(2025, 12, 31) 
        });

        ProjectRepositoryDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Project> { project });
        MapperDouble.Setup(m => m.Map<ProjectDTO>(It.IsAny<Project>()))
            .Returns((Project p) => new ProjectDTO(p.Id, p.Title, p.Acronym, p.PeriodDate));

        // Act
        var result1 = await ProjectService.GetAll();
        var result2 = await ProjectService.GetAll();

        // Assert
        Assert.True(result1.IsSuccess);
        Assert.True(result2.IsSuccess);
        
        // Verificar que são instâncias diferentes (referências diferentes)
        Assert.NotSame(result1.Value, result2.Value);
        
        // Mas com o mesmo conteúdo
        Assert.Equal(result1.Value.Count(), result2.Value.Count());
        Assert.Equal(result1.Value.First().Id, result2.Value.First().Id);
        
        // Verificar que o repositório foi chamado duas vezes
        ProjectRepositoryDouble.Verify(r => r.GetAllAsync(), Times.Exactly(2));
    }
 }
