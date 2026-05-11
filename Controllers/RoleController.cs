using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
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
    }
}
