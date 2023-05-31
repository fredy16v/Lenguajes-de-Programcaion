using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
	public class Categoria
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage = "El {0} debe tener entre {2} y {1} letras")]
		public string Nombre { get; set; }
		public TipoTransaccion TipoTransaccionId { get; set; }
		public int UsuarioId { get; set; }
	}
}
