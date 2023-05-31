using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
	public interface IRepositorioCategorias
	{
		Task Crear(Categoria categoria);
		Task<IEnumerable<Categoria>> Obtener(int usuarioId);
	}
}