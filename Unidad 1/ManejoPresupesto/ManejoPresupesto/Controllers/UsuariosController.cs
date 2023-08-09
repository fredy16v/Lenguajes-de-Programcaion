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

    public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Transacciones");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        var resultado = _signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, true, false).Result;

        if (!resultado.Succeeded)
        {
            ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            return View(modelo);
        }

        return RedirectToAction("Index", "Transacciones");
    }
}