using System.Text;
using Newtonsoft.Json;
using Xunit;
using Application.DTO;
using Domain.Models;
using WebApi.IntegrationTests.Helpers;
using Domain.Interfaces;
using System.Net;


namespace WebApi.IntegrationTests.Tests;

public class ProjectControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public ProjectControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task CreateProject_Returns201Created()
    {
        // Arrange
        // Create a random project payload
        var projectDTO = ProjectHelper.GenerateRandomProjectDto();

        // Act: Send the POST request to create the project
        var createdProjectDTO = await PostAndDeserializeAsync<ProjectDTO>("/api/Project", projectDTO);

        // Assert
        Assert.NotNull(createdProjectDTO);
        Assert.Equal(projectDTO.Title, createdProjectDTO.Title);
        Assert.Equal(projectDTO.Acronym, createdProjectDTO.Acronym);
        Assert.Equal(projectDTO.PeriodDate.InitDate, createdProjectDTO.PeriodDate.InitDate);
        Assert.Equal(projectDTO.PeriodDate.FinalDate, createdProjectDTO.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task CreateProject_WithInvalidAcronym_Returns400BadRequest()
    {
        // Arrange
        var invalidProjectDTO = ProjectHelper.GenerateRandomProjectDto();
        invalidProjectDTO.Acronym = "Invalid"; // contains lowercase letters

        // Act
        var response = await PostAsync("/api/Project", invalidProjectDTO);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("Acronym must be 1 to 10 characters long and contain only uppercase letters and digits.", body);
    }

    [Fact]
    public async Task UpdateProject_Returns200_AndUpdatesValues()
    {
        //Arrange: cria um projeto inicial
        var originalDto = ProjectHelper.GenerateRandomProjectDto();

        var createdProject = await PostAndDeserializeAsync<ProjectDTO>("/api/Project", originalDto);
        Assert.NotNull(createdProject);

        createdProject.Title = "UPDATED" + createdProject.Title;
        createdProject.Acronym = "UPD" + new Random().Next(100, 999);
        createdProject.PeriodDate = new PeriodDate(
        DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1)),
        DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(10))
    );

        //Act
        var updatedResponse = await PutAsync("/api/Project", createdProject);
        var updatedContent = await PutAndDeserialize<ProjectDTO>(updatedResponse);

        //Assert

        Assert.Equal(HttpStatusCode.OK, updatedResponse.StatusCode);
        Assert.Equal(createdProject.Title, updatedContent.Title);
        Assert.Equal(createdProject.Acronym, updatedContent.Acronym);
        Assert.Equal(createdProject.PeriodDate.InitDate, updatedContent.PeriodDate.InitDate);
        Assert.Equal(createdProject.PeriodDate.FinalDate, updatedContent.PeriodDate.FinalDate);
}
        
    }

    

   
