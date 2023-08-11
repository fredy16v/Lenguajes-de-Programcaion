using Dapper;
using ManejoPresupesto.Models;
using ManejoPresupesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupesto.Controllers
{
    public class TiposCuentaController : Controller
    {
        private readonly IRepositorioTiposCuenta repositorioTiposCuenta;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentaController(
                IRepositorioTiposCuenta repositorioTiposCuenta,
                IServicioUsuarios servicioUsuarios
            )
        {
            this.repositorioTiposCuenta = repositorioTiposCuenta;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuenta = await repositorioTiposCuenta.Obtener(usuarioId);

            return View(tiposCuenta);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId();
            tipoCuenta.Orden = 0;

            var yaExisteTipoCuenta =
                await repositorioTiposCuenta.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }

            await repositorioTiposCuenta.Crear(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuenta.ObtenerPorId(id, usuarioId);

            var editarTipoCuenta = new EditarTipoCuentaViewModel 
            {
                Id = tipoCuenta.Id,
                Nombre = tipoCuenta.Nombre,
                UsuarioId = usuarioId,
                Orden = tipoCuenta.Orden,
            };

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(editarTipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarTipoCuentaViewModel editarTipoCuenta) 
        {
            if (!ModelState.IsValid) 
            {
                return View(editarTipoCuenta);
            }

            await repositorioTiposCuenta.Editar(editarTipoCuenta);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerificarExisteCuenta(string nombre) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var yaExsisteTipoCuenta = await repositorioTiposCuenta.Existe(nombre, usuarioId);

            if (yaExsisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuenta.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);

        }

        [HttpPost]
        public async Task<IActionResult> BorrarConfirm(int id) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuenta.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuenta.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuenta.Obtener(usuarioId);

            var idsTiposCuenta = tiposCuentas.Select(x => x.Id);

            var idsTiposCuentaNoPertenecenAlUsuario = ids.Except(idsTiposCuenta).ToList();

            if (idsTiposCuentaNoPertenecenAlUsuario.Count > 0) 
            {
                return Forbid();
            }

            var tiposCuentasOrdenadas = ids.Select((valor, indice) =>
                new TipoCuenta() { Id =  valor, Orden = indice + 1 }
            ).AsEnumerable();

            await repositorioTiposCuenta.Ordenar(tiposCuentasOrdenadas);

            return Ok();
        }
    }
}
