using Dapper;
using ManejoPresupesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupesto.Servicios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration
                .GetConnectionString("DefaultConnection").ToString();
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync
                <Usuario>(
                "SELECT * FROM Usuarios WHERE EmailNormalizado = @EmailNormalizado;",
                new { emailNormalizado });
        }

        public async Task<int> CrearUsuario(Usuario modelo) 
        {
            using var connection = new SqlConnection(connectionString);

            modelo.EmailNormalizado = modelo.Email.ToUpper();

            var id = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Usuarios
                    (Email, EmailNormalizado, PasswordHash)
                    VALUES
                    (@Email, @EmailNormalizado, @PasswordHash);
                    SELECT SCOPE_IDENTITY();",
                modelo);

            return id;
        }
    }
}
