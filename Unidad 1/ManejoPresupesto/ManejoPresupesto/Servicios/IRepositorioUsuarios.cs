using ManejoPresupesto.Models;

namespace ManejoPresupesto.Servicios;

public interface IRepositorioUsuarios
{
    Task<int> CrearUsuarioAsync(Usuario usuario);
    Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
}