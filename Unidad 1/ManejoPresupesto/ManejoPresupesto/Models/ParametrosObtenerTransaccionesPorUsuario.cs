namespace ManejoPresupesto.Models
{
    public class ParametrosObtenerTransaccionesPorUsuario
    {
        public ParametrosObtenerTransaccionesPorUsuario()
        {
        }

        public int UsuarioId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}