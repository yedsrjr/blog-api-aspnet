using Azure;
using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class RoleController(BlogDataContext context) : Controller
    {
        [HttpGet("v1/roles")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var roles = await context.Roles
                        .AsNoTracking()
                        .ToListAsync();


                return Ok(new ResultViewModel<List<Role>>(roles));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Role>>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/roles/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var role = await context.Roles
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                if (role == null)
                {
                    return NotFound(new ResultViewModel<Role>("Conteúdo não encontrado"));
                }


                return Ok(new ResultViewModel<Role>(role));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/roles")]
        public async Task<IActionResult> PostAsync([FromBody] RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));
            }

            try
            {
                var role = new Role
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug,
                    Users = null
                };

                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();

                return Created($"v1/tags/{role.Id}", role);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<List<Role>>("05XE9 - Não foi possível incluir a role"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("05X02 - Falha interna no servidor"));
            }
        }
    }
}
