// using System.Text;
// using Newtonsoft.Json;
// using Xunit;
// using Application.DTO;
// using Domain.Models;
// using WebApi.IntegrationTests.Helpers;
// using Domain.Interfaces;
// using System.Net;


// namespace WebApi.IntegrationTests.Tests;

// public class ProjectControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
// {
//     public ProjectControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
//         : base(factory.CreateClient())
//     {
//     }
    
//     [Fact]
//     public async Task GetAllProjects_ReturnsCreatedProject()
//     {
//         // Arrange: cria um projeto para garantir que o GET tenha algo para devolver
       
//         // Act: faz o GET para ir buscar os projetos criados
//         var response = await Client.GetAsync("/api/Project");
//         var responseBody = await response.Content.ReadAsStringAsync();
//         var allProjects = JsonConvert.DeserializeObject<List<ProjectDTO>>(responseBody)!;

//         // Assert
//         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//         Assert.NotNull(allProjects);
//         Assert.Contains(allProjects, p =>
//             p.Id == createdProjectDTO.Id &&
//             p.Title == createdProjectDTO.Title &&
//             p.Acronym == createdProjectDTO.Acronym &&
//             p.PeriodDate.InitDate == createdProjectDTO.PeriodDate.InitDate &&
//             p.PeriodDate.FinalDate == createdProjectDTO.PeriodDate.FinalDate);
//     }
 
// }