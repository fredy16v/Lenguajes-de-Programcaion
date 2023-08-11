﻿using AutoMapper;
using ClosedXML.Excel;
using ManejoPresupesto.Models;
using ManejoPresupesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace ManejoPresupesto.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServicioReportes servicioReportes;
        private readonly ILogger<TransaccionesController> logger;
        private readonly IMapper mapper;

        public TransaccionesController(
            IServicioUsuarios servicioUsuarios,
            IRepositorioTransacciones repositorioTransacciones,
            IRepositorioCuentas repositorioCuentas,
            IRepositorioCategorias repositorioCategorias,
            IServicioReportes servicioReportes,
            ILogger<TransaccionesController> logger,
            IMapper mapper
            )
        {
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioTransacciones = repositorioTransacciones;
            this.repositorioCuentas = repositorioCuentas;
            this.repositorioCategorias = repositorioCategorias;
            this.servicioReportes = servicioReportes;
            this.logger = logger;
            this.mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int mes, int año) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var modelo = await servicioReportes.ObtenerReporteTransaccionesDetalladas(
                usuarioId, mes, año, ViewBag);

            return View(modelo);
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

            // Si tengo error regreso a la vista
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

        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId) 
        {
            var cuentas = await repositorioCuentas.ObtenerCuentas(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCategorias([FromBody] TipoTransaccion tipoTransaccion) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var categorias = await ObtenerCategorias(usuarioId, tipoTransaccion);

            return Ok(categorias);
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, 
                TipoTransaccion tipoTransaccion) 
        {
            var categorias = await repositorioCategorias.Obtener(usuarioId, tipoTransaccion);
            return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id, string urlRetorno = null) 
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
            modelo.UrlRetorno = urlRetorno;
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

            if (string.IsNullOrEmpty(modelo.UrlRetorno))
            {
                return RedirectToAction("Index");
            }
            else
            { 
                return LocalRedirect(modelo.UrlRetorno);
            }


        }

        [HttpPost]
        public async Task<IActionResult> Borrar(int id, string urlRetorno = null) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var transaccion = repositorioTransacciones.ObtenerPorId(id, usuarioId);

            if (transaccion is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTransacciones.Borrar(id);

            if (string.IsNullOrEmpty(urlRetorno))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(urlRetorno);
            }
        }

        public async Task<IActionResult> Semanal(int mes, int año) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            IEnumerable<ResultadoObtenerPorSemana> transaccionesPorSemana = await servicioReportes
                .ObtenerReporteSemanal(usuarioId, mes, año, ViewBag);

            var agrupado = transaccionesPorSemana.GroupBy(x => x.Semana)
                .Select(x => new ResultadoObtenerPorSemana 
                {
                    Semana = x.Key,
                    Ingresos = x.Where(x => x.TipoTransaccionId == TipoTransaccion.Ingreso)
                        .Select(x => x.Monto).FirstOrDefault(),
                    Gastos = x.Where(x => x.TipoTransaccionId == TipoTransaccion.Gasto)
                        .Select(x => x.Monto).FirstOrDefault(),
                }).ToList();

            if (año == 0 || mes == 0)
            {
                var hoy = DateTime.Today;
                año = hoy.Year;
                mes = hoy.Month;
            }

            var fechaReferencia = new DateTime(año, mes, 1);
            var diasDelMes = Enumerable.Range(1, fechaReferencia.AddMonths(1).AddDays(-1).Day);
            var diasSegmentados = diasDelMes.Chunk(7).ToList();

            for (int i = 0; i < diasSegmentados.Count; i++)
            {
                var semana = i + 1;
                var fechaInicio = new DateTime(año, mes, diasSegmentados[i].First());
                var fechaFin = new DateTime(año, mes, diasSegmentados[i].Last());

                var grupoSemana = agrupado.FirstOrDefault(x => x.Semana == semana);

                if (grupoSemana is null)
                {
                    agrupado.Add(new ResultadoObtenerPorSemana 
                    {
                        Semana = semana,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                    });
                }
                else
                {
                    grupoSemana.FechaInicio = fechaInicio;
                    grupoSemana.FechaFin = fechaFin;
                }
            }

            agrupado = agrupado.OrderByDescending(x => x.Semana).ToList();

            var modelo = new ReporteSemanalViewModel 
            {
                TransaccionesPorSemana = agrupado,
                FechaReferencia = fechaReferencia,
            };

            return View(modelo); 
        }

        public async Task<IActionResult> Mensual(int año) 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            if (año == 0 )
            {
                año = DateTime.Today.Year;
            }

            var transaccionesPorMes = await repositorioTransacciones
                .ObtenerPorMes(usuarioId, año);

            var transaccionesAgrupadas = transaccionesPorMes
                .GroupBy(x => x.Mes)
                .Select(x => new ResultadoObtenerPorMes 
                {
                    Mes = x.Key,
                    Ingreso = x.Where(x => x.TipoTransaccionId == TipoTransaccion.Ingreso)
                        .Select(x => x.Monto).FirstOrDefault(),
                    Gasto = x.Where(x => x.TipoTransaccionId == TipoTransaccion.Gasto)
                        .Select(x => x.Monto).FirstOrDefault(),
                }).ToList();

            for (int mes = 1; mes <= 12; mes++)
            {
                var transaccion = transaccionesAgrupadas.FirstOrDefault(x => x.Mes == mes);
                var fechaReferencia = new DateTime(año, mes, 1);

                if (transaccion is null)
                {
                    transaccionesAgrupadas.Add(new ResultadoObtenerPorMes
                    {
                        Mes = mes,
                        FechaReferencia = fechaReferencia
                    });
                }
                else 
                {
                    transaccion.FechaReferencia = fechaReferencia;
                }
            }

            transaccionesAgrupadas = transaccionesAgrupadas
                .OrderByDescending(x => x.Mes).ToList();

            var modelo = new ReporteMensualViewModel 
            {
                TransaccionesPorMes = transaccionesAgrupadas,
                Año = año
            };

            return View(modelo); 
        }

        public IActionResult Excel() { return View(); }

        [HttpGet]
        public async Task<FileResult> ExportarExcelPorMes(int mes, int año) 
        {
            var fechaInicio = new DateTime(año, mes, 1);
            var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var transacciones = await repositorioTransacciones
                .ObtenerPorUsuarioId(new ParametrosObtenerTransaccionesPorUsuario 
                {
                    UsuarioId = usuarioId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                });

            var nombreArchivo = $"Manejo Presupuesto - {fechaInicio.ToString("MMM yyyy")}.xlsx";
            return GenerarExcel(nombreArchivo, transacciones);
        }

        public IActionResult Calendario() { return View(); }


        private FileResult GenerarExcel
            (string nombreArchivo, IEnumerable<Transaccion> transacciones) 
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] 
            {
                new DataColumn("Fecha"),
                new DataColumn("Cuenta"),
                new DataColumn("Categoria"),
                new DataColumn("Nota"),
                new DataColumn("Monto"),
                new DataColumn("Ingreso / Gasto"),
            });

            foreach (var transaccion in transacciones)
            {
                dataTable.Rows.Add(
                    transaccion.FechaTransaccion,
                    transaccion.Cuenta,
                    transaccion.Categoria,
                    transaccion.Nota,
                    transaccion.Monto,
                    transaccion.TipoTransaccionId
                );
            }

            using (XLWorkbook wb = new XLWorkbook()) 
            {
                wb.Worksheets.Add(dataTable, "Transacciones");
                using (MemoryStream stream = new MemoryStream()) 
                {
                    wb.SaveAs( stream );
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }
            }
        }

    }
}
