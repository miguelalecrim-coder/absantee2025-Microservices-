

using Application.DTO;
using Application.Tests.ProjectServiceTests;
using Domain.Models;
using Moq;

public class ProjectServiceEditProject : ProjectServiceTestBase
{
    [Fact]
    public async Task EditProject_ShouldReturnSuccess()
    {
        //Arrange
        var projectId = Guid.NewGuid();
        var periodDate = new PeriodDate(DateOnly.MinValue, DateOnly.MaxValue);

        var originalProject = new Project(projectId, "OLD Title", "OLD", periodDate);
        var updatedProject = new Project(projectId, "New Title", "NEW", periodDate);

        var projectDTO = new ProjectDTO(projectId, "New Title", "NEW", periodDate);


        ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(projectId)).ReturnsAsync(originalProject);

        ProjectRepositoryDouble.Setup(x => x.UpdateProject(originalProject)).ReturnsAsync(updatedProject);

        PublisherDouble
        .Setup(p => p.PublishUpdatedProjectMessageAsync(projectId, "New Title", "NEW", periodDate))
        .Returns(Task.CompletedTask);

        MapperDouble
        .Setup(m => m.Map<ProjectDTO>(updatedProject))
        .Returns(projectDTO);


        //Act

        var result = await ProjectService.EditProject(projectDTO);

        //Assert   

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(projectDTO.Id, result.Value.Id);
        Assert.Equal(projectDTO.Title, result.Value.Title);
        Assert.Equal(projectDTO.Acronym, result.Value.Acronym);
        Assert.Equal(projectDTO.PeriodDate, result.Value.PeriodDate);

        ProjectRepositoryDouble.Verify(r => r.GetByIdAsync(projectId), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.UpdateProject(It.IsAny<Project>()), Times.Once);
        PublisherDouble.Verify(p => p.PublishUpdatedProjectMessageAsync(projectId, "New Title", "NEW", periodDate), Times.Once);
        MapperDouble.Verify(m => m.Map<ProjectDTO>(updatedProject), Times.Once);

    }

    [Fact]
    public async Task EditProject_ShouldReturnNotFound_WhenProjectDoesNotExiste()
    {
        //arrange

        var dto = new ProjectDTO(Guid.NewGuid(), "New Title", "NEW", new PeriodDate());
        ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(dto.Id)).ReturnsAsync((Project)null);

        //act

        var result = await ProjectService.EditProject(dto);

        //Assert

        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found", result.Error.Message);
    }

    [Fact]
    public async Task EditProject_ShouldReturnsInternalServerError_WhenUpdateProjectReturnsNull()
    {
        //Arrange

        var dto = new ProjectDTO(Guid.NewGuid(), "New Title", "NEW", new PeriodDate());
        var project = new Project(dto.Id, "Old Title", "OLD", dto.PeriodDate);

        ProjectRepositoryDouble.Setup(x => x.GetByIdAsync(dto.Id)).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(x => x.UpdateProject(It.IsAny<Project>())).ReturnsAsync((Project)null);

        //Act

        var result = await ProjectService.EditProject(dto);

        //Assert

        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update project", result.Error.Message);
    }



}