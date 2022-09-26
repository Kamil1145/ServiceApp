using Microsoft.AspNetCore.Mvc;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace ServiceApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(
            IRoleService roleService)
        {
            _roleService = roleService;
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Role>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet(nameof(GetRoles), Name =nameof(GetRoles))]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            try
            {
                return Ok(_roleService.GetRoles());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

    }


}

