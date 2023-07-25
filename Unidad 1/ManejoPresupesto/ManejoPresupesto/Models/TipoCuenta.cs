using ManejoPresupesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupesto.Models
{
    public class TipoCuenta : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El {0} es requerido")]
        //[PrimeraLetraMayuscula]
        [Remote(action: "VerificarExisteCuenta", "TiposCuenta")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        ////Pruebas de otras validaciones
        //[Required(ErrorMessage = "El {0} es requerido")]
        //[EmailAddress(ErrorMessage = "El {0} debe ser un correo electronico")]
        //public string Email { get; set; }

        //[Range(minimum: 18, maximum: 130, ErrorMessage = "El valor de {0} debe ser entre {1} y {2}")]
        //public int Edad { get; set; }

        //[Url(ErrorMessage = "El campo {0} debe tener el formato de una Url")]
        //public string Url { get; set; }

        //[Display(Name = "Tarjeta de Crédito")]
        //[CreditCard(ErrorMessage = "La {0} no tiene un formato valido")]
        //public string TarjetaCredito { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (Nombre != null && Nombre.Length > 0)  
            {
                var primeraLetra = Nombre.ToString()[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper()) 
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula", 
                        new[] { nameof(Nombre) });

                }
            }
        }
    }
}
