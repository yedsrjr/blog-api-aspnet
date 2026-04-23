using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Controllers;

[ApiController]
public class AccountController(BlogDataContext context, TokenService tokenService, EmailService emailService) : ControllerBase
{
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        }

        var user = await context.Users
                                .AsNoTracking()
                                .Include(x => x.Roles)
                                .FirstOrDefaultAsync(x => x.Email == model.Email);
        
        if (user == null)
        {
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));
        }

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
        {
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));
        }
        
        try
        {
            var token = tokenService.GenerateToken(user);
    
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna do Servidor"));
        }
    }

     [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        }

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        }; 
        
        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email,
                subject: "Bem Vindo ao blog!",
                body: $"Sua senha é <strong>{password}</strong>"
            );

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
            }));
        }
        catch(DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05x99 - Este E-mail já foi cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05x04 - Falha interna no servidor"));
        }
    }

    [Authorize]
    [HttpPost("v1/accounts/uploud-image")]
    public async Task<IActionResult> UploudImage([FromBody] UploudImageViewModel model)
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:imageV[a-z]+;base64,").Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (System.Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("05x04 - Falha interna do Servidor"));
        }

        var user =  await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
        
        if (user == null)
        {
            return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
        }

        user.Image = $"https://localhost:6140/images/{fileName}";

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("05x04 - Falha interna do Servidor"));
        }

        return Ok(new ResultViewModel<string>(data: $"Imagem alterada com sucesso: {user.Image}", errors: null));
    }
}