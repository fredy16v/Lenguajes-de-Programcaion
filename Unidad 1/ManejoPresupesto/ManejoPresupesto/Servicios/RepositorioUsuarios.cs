using Dapper;
using ManejoPresupesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupesto.Servicios;

public class RepositorioUsuarios: IRepositorioUsuarios
{
    private readonly string conncectionString;

    public RepositorioUsuarios(IConfiguration configuration)
    {
        conncectionString = configuration.GetConnectionString("DefaultConnection").ToString();
    }

    public async Task<int> CrearUsuarioAsync(Usuario usuario)
    {
        using var connection = new SqlConnection(conncectionString);
        await connection.OpenAsync();
 
        var query = @"INSERT INTO Usuarios (Email, EmailNormalizado, PasswordHash)
                      VALUES (@Email, @EmailNormalizado, @PasswordHash);
                      SELECT CAST(SCOPE_IDENTITY() as int)";
        var parametros = new
        {
            Email = usuario.Email,
            EmailNormalizado = usuario.EmailNormalizado,
            PasswordHash = usuario.PasswordHash
        };
        return await connection.QuerySingleAsync<int>(query, parametros);
    }

    public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
    {
        using var connection = new SqlConnection(conncectionString);
        return await connection.QuerySingleOrDefaultAsync<Usuario>(
            "SELECT * FROM Usuarios WHERE EmailNormalizado = @EmailNormalizado",
            new {EmailNormalizado = emailNormalizado}
        );
    }
}