using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
	public interface IRepositorioTransacciones
	{
		Task Crear(Transaccion transaccion);
	}
}