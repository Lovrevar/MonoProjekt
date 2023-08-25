using AutoMapper;
using MVC.Models.VehicleModel;
using Service;
using Service.Models;

namespace MVC.MappingProfiles
{
    public class VehicleModelProfile : Profile
    {
        public VehicleModelProfile()
        {
            CreateMap<VehicleModel, ModelDetailsVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.VehicleMake.Name))
                .ForMember(dest => dest.MakeAbrv, opt => opt.MapFrom(src => src.VehicleMake.Abrv));
            CreateMap<Paging<VehicleModel>, ModelListVM>()
                .ForMember(dest => dest.Models, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => src));
            CreateMap<CreateModelVM, VehicleModel>();
        }
    }
}