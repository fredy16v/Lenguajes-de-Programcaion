using System.ComponentModel.DataAnnotations;

namespace ManejoPresupesto.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [Display(Name = "Fecha de Transacción")]
        [DataType(DataType.Date)]
        public DateTime FechaTransaccion { get; set; } = DateTime.Today;
        //public DateTime FechaTransaccion { get; set; } = DateTime.Parse(DateTime.Now.ToString("g"));
        //public DateTime FechaTransaccion { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:MM tt"));
        public decimal Monto { get; set; }
        public string Nota { get; set; }

        [Display(Name = "Cuentas")]
        public int CuentaId { get; set; }

        [Display(Name = "Categorías")]
        public int CategoriaId { get; set; }

        [Display(Name = "Tipo de Transacción")]
        public TipoTransaccion TipoTransaccionId { get; set; } = TipoTransaccion.Ingreso;

        public string Cuenta { get; set; }
        public string Categoria { get; set; }
    }
}
