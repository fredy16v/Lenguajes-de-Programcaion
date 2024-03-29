using Microsoft.AspNetCore.Authentication;
using TareasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace TareasMVC.Controllers;

public class UsuariosController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public UsuariosController(UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Registro()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Registro(RegistroViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var usuario = new IdentityUser
        {
            Email = modelo.Email,
            UserName = modelo.Email
        };

        var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);
        if (resultado.Succeeded)
        {
            await signInManager.SignInAsync(usuario, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(modelo);
        }
    }
    
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password,
            modelo.RememberMe, lockoutOnFailure: false);
        if (resultado.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos");
            return View(modelo);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }
}