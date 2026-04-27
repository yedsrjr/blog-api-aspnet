using Blog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Posts
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "O Título é obrigatório")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O Sumário é obrigatório")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "O Body é obrigatório")]
        public string Body { get; set; }

        [Required(ErrorMessage = "O slug é obrigatório")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatório")]
        public int Category { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
