using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
	public class Transaccion
	{
		public int Id { get; set; }
		public int UsuarioId { get; set; }
		[Display(Name = "Fecha de Transaccion")]
		[DataType(DataType.Date)]
		public DateTime FechaTransaccion { get; set; } = DateTime.Today;
		//public DateTime FechaTransaccion { get; set; } = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
		public decimal Monto { get; set; }
		public string Nota { get; set; }
		[Display(Name = "Cuentas")]
		public int CuentaId { get; set; }
		[Display(Name = "Categorias")]
		public int CategoriaId { get; set; }
	}
}
