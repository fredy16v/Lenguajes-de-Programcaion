using ManejoPresupesto.Models;

namespace ManejoPresupesto.Servicios
{
    public interface IServicioReportes
    {
        Task<ReporteTransaccionesDetalladas> ObtenerReporteTransaccionesDetalladasPorCuenta(int usuarioId, int cuentaId, int mes, int año, dynamic ViewBag);
        
        Task<ReporteTransaccionesDetalladas> ObtenerReporteTransaccionesDetalladas(int usuarioId, int mes, int año, dynamic ViewBag);
        
        
    }
}
