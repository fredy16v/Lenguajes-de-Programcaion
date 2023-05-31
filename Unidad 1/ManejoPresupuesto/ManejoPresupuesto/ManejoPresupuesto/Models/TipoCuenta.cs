using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
	public class TipoCuenta
	{
		public int Id { get; set; }

		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "El {0} es requerido.")]
		[PrimeraLetraMayuscula]
		[Remote(action: "VerificarExisteCuenta", "TiposCuenta")]
		public string Nombre { get; set; }

		public int UsuarioId { get; set; }

		public int Orden { get; set; }

		//Puebas de otras validaciones
		//[Required(ErrorMessage = "El {0} es requerido")]
		//[EmailAddress(ErrorMessage = "El {0} es invalido")]
		//public string Email { get; set; }

		//[Range(minimum: 18, maximum: 130, ErrorMessage = "El valor de {0} debe ser entre {1} y {2}")]
		//public int Edad { get; set; }

		//[Url(ErrorMessage = "El campo {0} debe tener el formato de una Url")]
		//public string Url { get; set; }

		//[Display(Name = "Targeta de Credito")]
		//[CreditCard(ErrorMessage = "La {0} no tiene un formato valido")]
		//public string TargetaCredito { get; set; }
	}
}
