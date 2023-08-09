using System.ComponentModel.DataAnnotations;

namespace ManejoPresupesto.Models;

public class RegistroViewModel
{
    [Required(ErrorMessage = "El {0} es obligatorio")]
    [Display(Name = "Correo")]
    [EmailAddress(ErrorMessage = "Debe colocar un correo válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El {0} es obligatorio")]
    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirmar contraseña")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}