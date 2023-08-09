using ManejoPresupesto.Models;
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

    public IActionResult Logout()
    {
        throw new NotImplementedException();
    }

    public IActionResult Login()
    {
        throw new NotImplementedException();
    }
}