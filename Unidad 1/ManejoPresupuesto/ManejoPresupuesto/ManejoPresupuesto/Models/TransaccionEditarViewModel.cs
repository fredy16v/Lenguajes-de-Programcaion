namespace ManejoPresupuesto.Models
{
	public class TransaccionEditarViewModel : TransaccionCreacionViewModel
	{
		public int CuentaAnteriorId { get; set; }
		public decimal MontoAnterior { get; set; }
	}
}
