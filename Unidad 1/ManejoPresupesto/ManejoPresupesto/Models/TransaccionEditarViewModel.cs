namespace ManejoPresupesto.Models
{
    public class TransaccionEditarViewModel: TransaccionCreacionViewModel
    {
        public int CuentaAnteriorId { get; set; }
        public decimal MontoAnterior { get; set; }

        public string UrlRetorno { get; set; }
    }
}
