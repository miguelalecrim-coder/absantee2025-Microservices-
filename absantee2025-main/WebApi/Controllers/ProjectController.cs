using Application.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
       

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
            
        }
   
        // UC4: Como gestor de projetos, quero criar projeto
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Add(CreateProjectDTO projectDTO)
        {
            var result = await _projectService.Add(projectDTO);

            return result.ToActionResult();
        }

        [HttpPut]
        public async Task<ActionResult<ProjectDTO>> Update([FromBody] ProjectDTO projectDTO)
        {
            var result = await _projectService.EditProject(projectDTO);

            return result.ToActionResult();
        }

        
    }
}
