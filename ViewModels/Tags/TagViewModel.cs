using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.ViewModels.Tags;

public class TagViewModel
{
        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O Slug é obrigatório")]
        public string Slug { get; set; }
        public List<Post> Posts { get; set; }
}
