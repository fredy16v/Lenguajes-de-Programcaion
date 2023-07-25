using ManejoPresupesto.Models;

namespace ManejoPresupesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel modelo);
        Task Borrar(int id);
        Task Crear(Cuenta cuenta);
        Task<IEnumerable<Cuenta>> ObtenerCuentas(int usuarioId);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }
}