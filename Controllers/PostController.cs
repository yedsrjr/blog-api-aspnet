using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
}
