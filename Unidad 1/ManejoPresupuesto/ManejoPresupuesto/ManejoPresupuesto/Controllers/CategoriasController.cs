using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ManejoPresupuesto.Controllers
{
	public class CategoriasController : Controller
	{
		private readonly IRepositorioCategorias repositorioCategorias;
		private readonly IServicioUsuarios servicioUsuarios;

		public CategoriasController(IRepositorioCategorias repositorioCategorias, IServicioUsuarios servicioUsuarios)
		{
			this.repositorioCategorias = repositorioCategorias;
			this.servicioUsuarios = servicioUsuarios;
		}
		public async Task<IActionResult> Index()
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();

			return View(await repositorioCategorias.Obtener(usuarioId));
		}

		[HttpGet]
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Crear(Categoria categoria)
		{
			if (!ModelState.IsValid)
			{
				return View(categoria);
			}

			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			categoria.UsuarioId = usuarioId;

			await repositorioCategorias.Crear(categoria);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Editar(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var categotia = await repositorioCategorias.ObtenerPorId(id, usuarioId);

			if (categotia is null)
			{
				return RedirectToAction("NoENcontrado", "Home");
			}

			return View(categotia);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(Categoria modelo)
		{
			if (!ModelState.IsValid)
			{
				return View(modelo);
			}

			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var categotia = await repositorioCategorias.ObtenerPorId(modelo.Id, usuarioId);

			if (categotia is null)
			{
				return RedirectToAction("NoENcontrado", "Home");
			}

			modelo.UsuarioId = usuarioId;
			await repositorioCategorias.Editar(modelo);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Borrar(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var categotia = await repositorioCategorias.ObtenerPorId(id, usuarioId);

			if (categotia is null)
			{
				return RedirectToAction("NoENcontrado", "Home");
			}

			return View(categotia);
		}

		[HttpPost]
		public async Task<IActionResult> BorrarCategotia(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var categotia = await repositorioCategorias.ObtenerPorId(id, usuarioId);

			if (categotia is null)
			{
				return RedirectToAction("NoENcontrado", "Home");
			}

			await repositorioCategorias.Borrar(id);

			return RedirectToAction("Index");
		}
	}
}
