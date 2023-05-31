using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
	public class RepositorioCategorias : IRepositorioCategorias
	{
		public readonly string connectionString;
		public RepositorioCategorias(IConfiguration configuration)
		{
			this.connectionString = configuration.GetConnectionString("DefaultConnection").ToString();
		}

		public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			return await connection.QueryAsync<Categoria>
				(@"SELECT *
					FROM Categorias
					WHERE UsuarioId = @UsuarioId", new {usuarioId});
		}

		public async Task Crear(Categoria categoria)
		{
			using var connection = new SqlConnection(connectionString);
			var id = await connection.QuerySingleAsync<int>
				(@"INSERT INTO Categorias (Nombre, TipoTransaccionId, UsuarioId)
				VALUES (@Nombre, @TipoTransaccionId, @UsuarioId);
				SELECT SCOPE_IDENTITY();", categoria);

			categoria.Id = id;
		}
	}
}
