using Blog.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O Slug é obrigatório")]
        public string Slug { get; set; }
        public List<User> Users { get; set; }
    }
}
