using Dapper;
using ManejoPresupesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupesto.Servicios
{
    public class RepositorioCategorias : IRepositorioCategorias
    {
        public readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>
                (@"
                    SELECT *
                      FROM Categorias
                      WHERE UsuarioId = @UsuarioId
                    ", new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoTransaccion tipoTransaccionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>
                (@"
                    SELECT *
                      FROM Categorias
                      WHERE UsuarioId = @UsuarioId AND TipoTransaccionId = @TipoTransaccionId
                    ", new { usuarioId, tipoTransaccionId });
        }

        public async Task Crear(Categoria categoria) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                (@"
                    INSERT INTO Categorias (Nombre, TipoTransaccionId, UsuarioId)
                    VALUES (@Nombre, @TipoTransaccionId, @UsuarioId);
                    SELECT SCOPE_IDENTITY();
                ", categoria);

            categoria.Id = id;
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                @"
                SELECT 
	                *
                FROM Categorias
                WHERE Id = @Id AND UsuarioId = @UsuarioId;
                ", new { id, usuarioId });
        }

        public async Task Editar(Categoria categoria) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"
                UPDATE Categorias
	                SET Nombre = @Nombre,
	                TipoTransaccionId = @TipoTransaccionId
                WHERE Id = @Id;
                ", categoria);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Categorias WHERE Id = @Id;", new {id});
        }

    }
}
