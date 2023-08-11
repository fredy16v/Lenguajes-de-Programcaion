using Dapper;
using ManejoPresupesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupesto.Servicios
{
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection").ToString();
        }

        public async Task Crear(Cuenta cuenta) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Cuentas
                    (Nombre, TipoCuentaId, Balance, Descripcion)
	                    VALUES
                    (@Nombre, @TipoCuentaId, @Balance, @Descripcion);
                    SELECT SCOPE_IDENTITY()",
                    cuenta);
        }

        public async Task<IEnumerable<Cuenta>> ObtenerCuentas(int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>
                (@"SELECT
	                cu.Id,
	                cu.Nombre,
	                cu.Balance,
	                tc.Nombre AS TipoCuenta
                FROM Cuentas cu 
                INNER JOIN TiposCuenta tc 
                ON tc.Id = cu.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId
                ORDER BY Orden",
                new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>
                (@"SELECT 
	                cu.Id,
	                cu.Nombre,
	                cu.Balance,
	                cu.Descripcion,
	                cu.TipoCuentaId
                FROM Cuentas AS cu
                INNER JOIN TiposCuenta AS tc
                ON tc.Id = cu.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId AND cu.Id = @Id;",
                new { id, usuarioId});
        }

        public async Task Actualizar(CuentaCreacionViewModel modelo) 
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(
                @"UPDATE Cuentas
                    SET Nombre = @Nombre,
	                    Balance = @Balance,
	                    Descripcion = @Descripcion,
	                    TipoCuentaId = @TipoCuentaId
                    WHERE Id = @Id", modelo);
        
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Cuentas WHERE Id = @Id", new { id });
        }

    }
}
