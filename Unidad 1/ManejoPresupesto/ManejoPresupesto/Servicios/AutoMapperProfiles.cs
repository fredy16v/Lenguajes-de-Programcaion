using AutoMapper;
using ManejoPresupesto.Models;

namespace ManejoPresupesto.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
            CreateMap<Transaccion, TransaccionEditarViewModel>().ReverseMap();
        }
    }
}
