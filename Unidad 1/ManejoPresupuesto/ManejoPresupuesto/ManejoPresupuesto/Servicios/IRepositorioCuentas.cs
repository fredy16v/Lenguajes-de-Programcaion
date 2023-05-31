using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
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