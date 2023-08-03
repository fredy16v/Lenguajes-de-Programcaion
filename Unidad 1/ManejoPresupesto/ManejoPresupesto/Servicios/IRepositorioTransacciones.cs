using ManejoPresupesto.Models;

namespace ManejoPresupesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task Editar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ParametrosTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(ParametrosObtenerTransaccionesPorUsuario modelo);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametrosObtenerTransaccionesPorUsuario modelo);
        Task<IEnumerable<ResultadoObtenerPorMes>> ObtenerPorMes(int usuarioId, int año);
    }
}