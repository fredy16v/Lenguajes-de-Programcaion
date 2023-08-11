using System.Security.Claims;

namespace ManejoPresupesto.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly HttpContext httpContent;
        
        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            httpContent = httpContextAccessor.HttpContext;
        }
        public int ObtenerUsuarioId() 
        {
            if (httpContent.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContent.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
        }
    }
}
