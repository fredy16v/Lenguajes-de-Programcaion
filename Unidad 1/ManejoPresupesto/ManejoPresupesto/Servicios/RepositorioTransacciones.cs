﻿using Dapper;
using ManejoPresupesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupesto.Servicios
{
    public class RepositorioTransacciones: IRepositorioTransacciones
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

                },
                commandType: System.Data.CommandType.StoredProcedure);
            
            transaccion.Id = id;
        }

        public async Task Editar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId) 
        {
            using var connetion = new SqlConnection(connectionString);

            await connetion.ExecuteAsync("Transacciones_Editar", new {
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
                (@"
                SELECT 
	                trn.*,
	                cat.TipoTransaccionId
                FROM Transacciones trn
	                INNER JOIN Categorias cat
	                ON cat.Id = trn.CategoriaId
                WHERE trn.Id = @Id AND trn.UsuarioId = @UsuarioId
                ", new { id, usuarioId });
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transacciones_Borrar", 
                new { id },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ParametrosTransaccionesPorCuenta modelo) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>
                (@"
                    SELECT
	                    trn.Id,
	                    trn.FechaTransaccion,
                        trn.Monto,
	                    cat.Nombre AS Categoria,
	                    cue.Nombre AS Cuenta,
	                    cat.TipoTransaccionId
                    FROM Transacciones trn
	                    INNER JOIN Categorias cat ON cat.Id = trn.CategoriaId
	                    INNER JOIN Cuentas cue ON cue.Id = trn.CuentaId
                    WHERE trn.CuentaId = @CuentaId
	                    AND trn.UsuarioId = @UsuarioId
	                    AND trn.FechaTransaccion BETWEEN @FechaInicio AND @FechaFin;
                ", modelo);
        }
        
        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametrosObtenerTransaccionesPorUsuario modelo) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>
            (@"
                    SELECT
	                    trn.Id,
	                    trn.FechaTransaccion,
                        trn.Monto,
	                    cat.Nombre AS Categoria,
	                    cue.Nombre AS Cuenta,
	                    cat.TipoTransaccionId
                    FROM Transacciones trn
	                    INNER JOIN Categorias cat ON cat.Id = trn.CategoriaId
	                    INNER JOIN Cuentas cue ON cue.Id = trn.CuentaId
                    WHERE trn.UsuarioId = @UsuarioId
	                    AND trn.FechaTransaccion BETWEEN @FechaInicio AND @FechaFin;
                ", modelo);
        }
    }
}