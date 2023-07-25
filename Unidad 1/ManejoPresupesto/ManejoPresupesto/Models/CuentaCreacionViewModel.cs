using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupesto.Models
{
    public class CuentaCreacionViewModel : Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuenta { get; set; }
    }
}
