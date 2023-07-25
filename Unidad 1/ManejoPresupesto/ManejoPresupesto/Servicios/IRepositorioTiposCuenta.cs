using ManejoPresupesto.Models;

namespace ManejoPresupesto.Servicios
{
    public interface IRepositorioTiposCuenta
    {
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task Editar(EditarTipoCuentaViewModel tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenadas);
    }
}