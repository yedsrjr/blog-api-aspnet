using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class UploudImageViewModel
{
    [Required(ErrorMessage = "Imagem Inválida")]
    public string Base64Image {get; set;}
}
