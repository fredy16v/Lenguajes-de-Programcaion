
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Transacciones_Insertar 
	@UsuarioId INT,
	@FechaTransaccion DATE,
	@Monto DECIMAL(18,2),
	@CategoriaId INT,
	@CuentaId INT,
	@Nota NVARCHAR(1000)
AS
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO Transacciones (UsuarioId, FechaTransaccion, Monto, CategoriaId, CuentaId, Nota)
	VALUES (@UsuarioId, @FechaTransaccion, ABS(@Monto), @CategoriaId, @CuentaId, @Nota);

	UPDATE Cuentas
	SET Balance += @Monto
	WHERE Id = @CuentaId

	SELECT SCOPE_IDENTITY();

END
GO
