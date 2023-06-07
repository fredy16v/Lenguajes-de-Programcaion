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
				transaccion, commandType: System.Data.CommandType.StoredProcedure);

			transaccion.Id = id;
		} 
	}
}
