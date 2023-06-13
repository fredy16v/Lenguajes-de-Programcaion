using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace ManejoPresupuesto.Models
{
	public class TransaccionCreacionViewModel : Transaccion
	{
		public IEnumerable<SelectListItem> Cuentas { get; set; }
		public IEnumerable<SelectListItem> Categorias { get; set; }
	}
}
