using AutoMapper;
using ManejoPresupesto.Models;
using ManejoPresupesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ManejoPresupesto.Controllers
{
    public class CuentasController : Controller 
    {
        private readonly IRepositorioTiposCuenta repositorioTiposCuenta;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly IMapper mapper;

        public CuentasController(
            IRepositorioTiposCuenta repositorioTiposCuenta,
            IServicioUsuarios servicioUsuarios,
            IRepositorioCuentas repositorioCuentas,
            IRepositorioTransacciones repositorioTransacciones,
            IMapper mapper
            )
        {
            this.repositorioTiposCuenta = repositorioTiposCuenta;
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioCuentas = repositorioCuentas;
            this.repositorioTransacciones = repositorioTransacciones;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await repositorioCuentas.ObtenerCuentas(usuarioId);

            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndexCuentasViewModel
                {
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()
                }).ToList();


            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModel();

            modelo.TiposCuenta = await ObtenerTiposCuenta(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel modelo) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            if (!ModelState.IsValid) 
            {
                modelo.TiposCuenta = await ObtenerTiposCuenta(usuarioId);
                return View(modelo);
            }

            var tipoCuenta = await repositorioTiposCuenta.ObtenerPorId(modelo.TipoCuentaId, usuarioId);
            
            if (tipoCuenta is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Crear(modelo);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<CuentaCreacionViewModel>(cuenta);

            //var modelo = new CuentaCreacionViewModel 
            //{
            //    Id = cuenta.Id,
            //    Nombre = cuenta.Nombre,
            //    Descripcion = cuenta.Descripcion,
            //    Balance = cuenta.Balance,
            //    TipoCuentaId = cuenta.TipoCuentaId,
            //};

            modelo.TiposCuenta = await ObtenerTiposCuenta(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel modelo) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(modelo.Id, usuarioId);

            if (!ModelState.IsValid)
            {
                modelo.TiposCuenta = await ObtenerTiposCuenta(usuarioId);
                return View(modelo);
            }

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var tipoCuenta = await repositorioTiposCuenta.ObtenerPorId(modelo.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Actualizar(modelo);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);

        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Borrar(id);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id, int mes, int año) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            DateTime fechaInicio;
            DateTime fechaFin;

            if (mes <= 0 || mes > 12 || año <= 1900)
            {
                var hoy = DateTime.Today;
                fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            }
            else 
            {
                fechaInicio = new DateTime(año, mes, 1);
            }

            fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var parametrosTransaccionesPorCuenta = new ParametrosTransaccionesPorCuenta 
            {
                CuentaId = cuenta.Id,
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
            };

            var transacciones = await repositorioTransacciones
                .ObtenerPorCuentaId(parametrosTransaccionesPorCuenta);

            var modelo = new ReporteTransaccionesDetalladas();
            ViewBag.Cuenta = cuenta.Nombre;

            var transaccionesPorFecha = transacciones.OrderByDescending(x => x.FechaTransaccion)
                .GroupBy(x => x.FechaTransaccion)
                .Select(grupo => new ReporteTransaccionesDetalladas.TransaccionesPorFecha {
                    FechaTransacion = grupo.Key,
                    Transacciones = grupo.AsEnumerable(),                    
                });

            modelo.TransaccionesAgrupadas = transaccionesPorFecha;
            modelo.FechaInicio = fechaInicio;
            modelo.FechaFin = fechaFin;

            ViewBag.mesAnterior = fechaInicio.AddMonths(-1).Month;
            ViewBag.añoAnterior = fechaInicio.AddMonths(-1).Year;

            ViewBag.mesPosterior = fechaInicio.AddMonths(1).Month;
            ViewBag.añoPosterior = fechaInicio.AddMonths(1).Year;

            ViewBag.urlRetorno = HttpContext.Request.Path + HttpContext.Request.QueryString;

            return View(modelo);
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuenta(int usuarioId)
        {
            var tiposCuenta = await repositorioTiposCuenta.Obtener(usuarioId);
            return tiposCuenta.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
