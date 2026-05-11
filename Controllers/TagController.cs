using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers;

[ApiController]
public class TagController(BlogDataContext context, IMemoryCache cache) : ControllerBase
{

    // CACHE: Atualmente usando IMemoryCache (cache em memória local).
    // Funciona bem para instância única (ambiente atual).
    // Se o projeto escalar para múltiplos servidores / containers,
    // migrar para IDistributedCache com Redis para garantir
    // consistência entre instâncias.
    [HttpGet("v1/tags")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var tags = cache.GetOrCreate("TagsCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetTags();
                });
                return Ok(new ResultViewModel<List<Tag>>(tags));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Tag>>("05X02 - Falha interna no servidor"));
            }
        }
        private List<Tag> GetTags()
        {
            return context.Tags.ToList();
        }

    [Authorize]
    [HttpPost("v1/tags")]
    public async Task<IActionResult> PostAsync([FromBody] TagViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<Tag>(ModelState.GetErrors()));
        }

        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

        if (user == null)
        {
            return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
        }

        try
        {
            var tag = new Tag
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug,
                Posts = null
            };

            await context.Tags.AddAsync(tag);
            await context.SaveChangesAsync();
            cache.Remove("TagsCache");

            return Created($"v1/tags/{tag.Id}", tag);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("05XE9 - Não foi possível incluir a tag"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("05X10 - Falha interna no servidor"));
        }
    }

    [Authorize]
    [HttpPut("v1/tags/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] TagViewModel model)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
            {
                return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
            }

            var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null)
            {
                return NotFound(new ResultViewModel<Tag>("Conteúdo não encontrado"));
            }

            tag.Name = model.Name;
            tag.Slug = model.Slug.ToLower();

            context.Tags.Update(tag);
            await context.SaveChangesAsync();
            cache.Remove("TagsCache");

            return Ok(new ResultViewModel<Tag>(tag));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("05XE9 - Não foi possível incluir a Tag"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("05X10 - Falha interna no servidor"));
        }
    }

    [Authorize]
    [HttpDelete("v1/tags/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
            {
                return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
            }

            var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null)
            {
                return NotFound(new ResultViewModel<Tag>("Conteúdo não encontrado"));
            }

            context.Tags.Remove(tag);
            await context.SaveChangesAsync();
            cache.Remove("TagsCache");

            return Ok(new ResultViewModel<Tag>(tag));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("05XE9 - Não foi possível deletar a Tag"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("05X10 - Falha interna no servidor"));
        }
    }

}