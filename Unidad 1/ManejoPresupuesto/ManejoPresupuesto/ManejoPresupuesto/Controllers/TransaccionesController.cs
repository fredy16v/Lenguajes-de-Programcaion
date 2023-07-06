using AutoMapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Transactions;

namespace ManejoPresupuesto.Controllers
{
	public class TransaccionesController : Controller
	{
		private readonly IServicioUsuarios servicioUsuarios;
		private readonly IRepositorioTransacciones repositorioTransacciones;
		private readonly IRepositorioCuentas repositorioCuentas;
		private readonly IRepositorioCategorias repositorioCategorias;
		private readonly IMapper mapper;

		public TransaccionesController(IServicioUsuarios servicioUsuarios, 
			IRepositorioTransacciones repositorioTransacciones,
			IRepositorioCuentas repositorioCuentas,
			IRepositorioCategorias repositorioCategorias,
			IMapper mapper)
		{
			this.servicioUsuarios = servicioUsuarios;
			this.repositorioTransacciones = repositorioTransacciones;
			this.repositorioCuentas = repositorioCuentas;
			this.repositorioCategorias = repositorioCategorias;
			this.mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
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
				modelo.Cuentas = await ObtenerCuentas(usuarioId);
				modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoTransaccionId);
				return View(modelo);
			}

			var cuenta = await repositorioCuentas.ObtenerPorId(modelo.CuentaId, usuarioId);
			if (cuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			var categoria = await repositorioCategorias.ObtenerPorId(modelo.CategoriaId, usuarioId);
			if (categoria is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			modelo.UsuarioId = usuarioId;

			if (modelo.TipoTransaccionId == TipoTransaccion.Gasto)
			{
				modelo.Monto *= -1;
			}

			await repositorioTransacciones.Crear(modelo);

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

		[HttpGet]
		public async Task<IActionResult> Editar(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			
			var transaccion = await repositorioTransacciones.ObtenerPorId(id, usuarioId);
			if (transaccion is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			var modelo = mapper.Map<TransaccionEditarViewModel>(transaccion);

			modelo.MontoAnterior = modelo.Monto;
			if (modelo.TipoTransaccionId == TipoTransaccion.Gasto)
			{
				modelo.MontoAnterior = modelo.Monto * -1;
			}

			modelo.CuentaAnteriorId = modelo.CuentaId;

			modelo.Cuentas = await ObtenerCuentas(usuarioId);
			modelo.Categorias = await ObtenerCategorias(usuarioId, transaccion.TipoTransaccionId);

			return View(modelo);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(TransaccionEditarViewModel modelo)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			if (!ModelState.IsValid)
			{
				modelo.Cuentas = await ObtenerCuentas(usuarioId);
				modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoTransaccionId);

				return View(modelo);
			}

			var cuenta = await repositorioCuentas.ObtenerPorId(modelo.CuentaId, usuarioId);
			if (cuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			var categoria = await repositorioCategorias.ObtenerPorId(modelo.CategoriaId, usuarioId);
			if (categoria is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			var transaccion = mapper.Map<Transaccion>(modelo);
			if (modelo.TipoTransaccionId == TipoTransaccion.Gasto)
			{
				transaccion.Monto *= -1;
			}

			await repositorioTransacciones.Editar(transaccion, modelo.MontoAnterior, modelo.CuentaAnteriorId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> Borrar(int id) //para borrar desde la vista de editar
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var transaccion = repositorioTransacciones.ObtenerPorId(id, usuarioId);

			if (transaccion is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			await repositorioTransacciones.Borrar(id);

			return RedirectToAction("Index");
		}
	}
}
