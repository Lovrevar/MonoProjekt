using AutoMapper;
using MVC.Models.VehicleModel;
using Service;

namespace MVC.Models.MappingProfiles
{
    public class VehicleModelProfile : Profile
    {
        public VehicleModelProfile()
        {
            CreateMap<Service.Models.VehicleModel, ModelDetailsVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.VehicleMake.Name))
                .ForMember(dest => dest.MakeAbrv, opt => opt.MapFrom(src => src.VehicleMake.Abrv));
            CreateMap<Paging<Service.Models.VehicleModel>, ModelListVM>()
                .ForMember(dest => dest.Models, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => src));
            CreateMap<Service.Models.VehicleModel, CreateModelVm>().ReverseMap();
            CreateMap<Service.Models.VehicleModel, UpdateModelVm>().ReverseMap();
        }
    }
}