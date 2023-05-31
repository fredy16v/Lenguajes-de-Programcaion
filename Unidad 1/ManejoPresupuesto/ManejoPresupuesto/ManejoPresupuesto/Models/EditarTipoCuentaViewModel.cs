using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
	public class EditarTipoCuentaViewModel : TipoCuenta
	{
		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "El {0} es requerido.")]
		[PrimeraLetraMayuscula]
		public new string Nombre { get; set; }
	}
}
