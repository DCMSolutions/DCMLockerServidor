using AutoMapper;
using DCMLockerServidor.Shared.Models;
using DCMLockerServidor.Shared;

namespace DCMLockerServidor.Server.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Tlockermap a box
            CreateMap<TLockerMapDTO, Box>()
              .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ForMember(dest => dest.IdFisico, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Puerta, opt => opt.MapFrom(src => src.Puerta))
              .ForMember(dest => dest.Ocupacion, opt => opt.MapFrom(src => src.Ocupacion))
              .ForMember(dest => dest.Libre, opt => opt.MapFrom(src => src.Libre))
              .ForMember(dest => dest.LastUpdateTime, opt => opt.Ignore()) // Ignora el mapeo de LastUpdateTime
              .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => src.Enable))
              .ForMember(dest => dest.IdSize, opt => opt.MapFrom(src => src.Size)) // Ignora el mapeo de IdSize
              .ForMember(dest => dest.Box1, opt => opt.Ignore()) // Ignora el mapeo de Box1 y cualquier otra propiedad que no coincida
              .ForMember(dest => dest.Status, opt => opt.Ignore()) // Ignora el mapeo de Status y cualquier otra propiedad que no coincida
              .ForMember(dest => dest.IdLockerNavigation, opt => opt.Ignore()) // Ignora el mapeo de IdLockerNavigation
              .ForMember(dest => dest.IdSizeNavigation, opt => opt.Ignore()) // Ignora el mapeo de IdSizeNavigation
              .ForMember(dest => dest.Tokens, opt => opt.Ignore()); // Ignora el mapeo de Tokens y cualquier otra propiedad que no 
            #endregion

            #region Size a SizeDTO
            CreateMap<Size, SizeDTO>()
                .ForMember(dest => dest.Cantidad, opt => opt.Ignore());
            
            CreateMap<SizeDTO, Size>();
            #endregion

        }

    }
}
