using ManejoPresupesto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupesto.Controllers;

public class UsuariosController : Controller
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly HttpContext _httpContext;

    public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
        HttpContext httpContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContext = httpContext;
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registro(RegistroViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        var usuario = new Usuario
        {
            Email = modelo.Email
        };

        var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
        if (!resultado.Succeeded)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        await _signInManager.SignInAsync(usuario, isPersistent: true);
        return RedirectToAction("Index", "Transacciones");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Transacciones");
    }

    [HttpPost]
    public IActionResult Login()
    {
        throw new NotImplementedException();
    }
}