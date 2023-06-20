using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
	public class TransaccionesController : Controller
	{
		private readonly IServicioUsuarios servicioUsuarios;
		private readonly IRepositorioTransacciones repositorioTransacciones;
		private readonly IRepositorioCuentas repositorioCuentas;
		private readonly IRepositorioCategorias repositorioCategorias;

		public TransaccionesController(IServicioUsuarios servicioUsuarios, 
			IRepositorioTransacciones repositorioTransacciones,
			IRepositorioCuentas repositorioCuentas,
			IRepositorioCategorias repositorioCategorias)
		{
			this.servicioUsuarios = servicioUsuarios;
			this.repositorioTransacciones = repositorioTransacciones;
			this.repositorioCuentas = repositorioCuentas;
			this.repositorioCategorias = repositorioCategorias;
		}

		[HttpGet]
		public async Task<IActionResult> Crear()
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();

			var modelo = new TransaccionCreacionViewModel();
			modelo.Cuentas = await ObtenerCuentas(usuarioId);
			modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoTransaccionId);

			return View(modelo);
		}

		[HttpPost]
		public async Task<IActionResult> Crear(TransaccionCreacionViewModel modelo)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();

			if (!ModelState.IsValid)
			{
				return View(modelo);
			}

			return RedirectToAction("Index");
		}

		private  async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
		{
			var cuentas = await repositorioCuentas.ObtenerCuentas(usuarioId);
			return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
		}

		[HttpPost]
		public async Task<IActionResult> ObtenerCategorias([FromBody] TipoTransaccion tipoTransaccion)//Porque la informacion se manda atravex del bodyen la vista de crear en el query de js
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var categorias = await ObtenerCategorias(usuarioId, tipoTransaccion);

			return Ok(categorias);
		}

		private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, TipoTransaccion tipoTransaccion)
		{
			var categorias = await repositorioCategorias.Obtener(usuarioId, tipoTransaccion);

			return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
		}
	}
}
