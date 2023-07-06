using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
	public interface IRepositorioTransacciones
	{
		Task Borrar(int id);
		Task Crear(Transaccion transaccion);
		Task Editar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId);
		Task<Transaccion> ObtenerPorId(int id, int usuarioId);
	}
}