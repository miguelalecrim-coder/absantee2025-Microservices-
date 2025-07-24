using Application.DTO;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;


        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAll()
        {
            var result = await _projectService.GetAll();

            return result.ToActionResult();
        }


        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectDTO>> GetById(Guid projectId)
        {
            var result = await _projectService.GetProjectById(projectId);

            return result.ToActionResult();
        }      
    }
}
