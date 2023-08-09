using System.ComponentModel.DataAnnotations;

namespace ManejoPresupesto.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "El {0} es obligatorio")]
    [Display(Name = "Correo")]
    [EmailAddress(ErrorMessage = "Debe colocar un correo válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El {0} es obligatorio")]
    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Recuerdame")]
    public bool RememberMe { get; set; }
}