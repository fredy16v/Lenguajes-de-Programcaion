using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
	public class RepositorioTiposCuenta : IRepositorioTiposCuenta
	{
		private readonly IConfiguration configuration;
		private readonly string connectionString;

		public RepositorioTiposCuenta(IConfiguration configuration)
		{
			this.configuration = configuration;
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			return await connection.QueryAsync<TipoCuenta>
				(@"SELECT Id, Nombre, Orden
				FROM TiposCuenta
				WHERE UsuarioId = @UsuarioId", new { usuarioId });
		}

		public async Task Crear (TipoCuenta tipoCuenta)//Insertar un registro a la base de datos. async es para hacer una tarea asincrona
		{
			using var connection = new SqlConnection(connectionString);
			var id = await connection.QuerySingleAsync<int>
				("TiposCuenta_Insertar", new { UsuarioId = tipoCuenta.UsuarioId, Nombre = tipoCuenta.Nombre }, 
				commandType: System.Data.CommandType.StoredProcedure);

		}

		public async Task Editar(EditarTipoCuentaViewModel tipoCuenta)
		{
			using var connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync
				(@"UPDATE TiposCuenta 
				SET Nombre = @Nombre
				WHERE Id = @Id", tipoCuenta);
		}

		public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			return await connection.QueryFirstOrDefaultAsync<TipoCuenta>
				(@"SELECT Id, Nombre, Orden
				FROM TiposCuenta
				WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
		}

		public async Task<bool> Existe (string nombre, int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			var existe = await connection.QueryFirstOrDefaultAsync<int>
				(@"SELECT 1
				FROM TiposCuenta
				WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId", 
				new {nombre, usuarioId});

			return existe == 1;
		}

		public async Task Borrar (int id)
		{
			using var connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync("DELETE FROM TiposCuenta WHERE Id = @Id", new { id });
		}

		public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenadas)
		{
			var query = "UPDATE TiposCuenta SET Orden = @Orden WHERE Id = @Id;";
			using var connection = new SqlConnection(connectionString);

			await connection.ExecuteAsync(query, tipoCuentasOrdenadas);
		}
	}
}
