using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace ManejoPresupuesto.Servicios
{
	public class RepositorioTransacciones : IRepositorioTransacciones
	{
		private readonly string connectionString;
		public RepositorioTransacciones(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task Crear(Transaccion transaccion)
		{
			using var connection = new SqlConnection(connectionString);
			var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar", 
				new
				{
					transaccion.UsuarioId,
					transaccion.FechaTransaccion,
					transaccion.Monto,
					transaccion.CategoriaId,
					transaccion.CuentaId,
					transaccion.Nota
				}, commandType: System.Data.CommandType.StoredProcedure);

			transaccion.Id = id;
		} 

		public async Task Editar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId)
		{
			using var connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync("Transacciones_Editar", new
			{
				transaccion.Id,
				transaccion.FechaTransaccion,
				transaccion.Monto,
				montoAnterior,
				transaccion.CuentaId,
				cuentaAnteriorId,
				transaccion.CategoriaId,
				transaccion.Nota
			}, commandType: System.Data.CommandType.StoredProcedure);
		}

		public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			return await connection.QueryFirstOrDefaultAsync<Transaccion>
				(@"SELECT 
					trn.*,
					cat.TipoTransaccionId
				FROM Transacciones trn
					INNER JOIN Categorias cat
					ON cat.Id = trn.CategoriaId
					WHERE trn.Id = @Id AND trn.UsuarioId = @UsuarioId", new {id, usuarioId});
		}

		public async Task Borrar(int id)
		{
			using var connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync("Transacciones_Borrar", new { id }, commandType: System.Data.CommandType.StoredProcedure);
		}
	}
}
