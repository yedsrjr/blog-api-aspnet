using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers;

[ApiController]
public class TagController(BlogDataContext context, IMemoryCache cache) : ControllerBase
{
    [HttpGet("v1/tags")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                // var categories = await context.Categories.ToListAsync();
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
}

