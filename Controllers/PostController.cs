using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers;

[ApiController]
public class PostController(BlogDataContext context) : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync(int page = 0, int pageSize = 25)
    {
        var count = await context.Posts.AsNoTracking().CountAsync();

        var posts = await context.Posts
        .AsNoTracking()
        .Include(x => x.Category)
        .Include(x => x.Author)
        .Select(x => new ListPostsViewModel
        {
            Id = x.Id,
            Title = x.Title,
            Summary = x.Summary,
            Slug = x.Slug,
            LastUpdateDate = x.LastUpdateDate,
            Category = x.Category.Name,
            Author = $"{x.Author.Name} - {x.Author.Email}"
        })
        .Skip(page * pageSize)
        .Take(pageSize)
        .OrderByDescending(x => x.LastUpdateDate)
        .ToListAsync();

        try
        {
            return Ok(new ResultViewModel<dynamic>(new
        {
            total = count,
            page, pageSize,
            posts
        }));
        }
        catch (System.Exception)
        {
            
            return StatusCode(500, new ResultViewModel<List<Post>>("05x04 - Falha interna do servidor"));
        }
    }

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var posts = await context.Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .ThenInclude(x => x.Roles)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (posts == null)
            {
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));
            }

            return Ok(new ResultViewModel<Post>(posts));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Post>("05X04 - Falha interna do servidor"));
        }
        
    }

    [HttpGet("v1/posts/category/{category}")]
    public async Task<IActionResult> GetByCategoryAsync(string category, int page = 0, int pageSize = 25)
    {
        try
        {
            var  count = await context.Posts.AsNoTracking().CountAsync();
            var posts = await context.Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category.Slug == category)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} - ({x.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            if (posts == null)
            {
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));
            }

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count, 
                page, 
                pageSize, 
                posts
            }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("05X04 - Falha interna do servidor"));
        }
        
    }

    [Authorize]
    [HttpPost("v1/posts")]
    public async Task<IActionResult> PostAsync([FromBody] PostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<Post>(ModelState.GetErrors()));
        }

        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

        if (user == null)
        {
            return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
        }

        try
        {
            var post = new Post
            {
                Id = 0,
                Title = model.Title,
                Summary = model.Summary,
                Body = model.Body,
                Slug = model.Slug.ToLower(),
                Category = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Category),
                Author = user,
                Tags = null
            };

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            return Created($"v1/posts/{post.Id}", post);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("05XE9 - Não foi possível incluir a categoria"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("05X10 - Falha interna no servidor"));
        }
    }
}
