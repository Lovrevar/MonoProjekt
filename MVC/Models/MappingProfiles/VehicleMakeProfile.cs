using AutoMapper;
using MVC.Models.VehicleMake;
using Service;

namespace MVC.Models.MappingProfiles
{
    public class VehicleMakeProfile : Profile
    {
        public VehicleMakeProfile()
        {
            CreateMap<Service.Models.VehicleMake, MakeDetailsVM>();
            CreateMap<Service.Models.VehicleMake, CreateMakeVM>().ReverseMap();
            CreateMap<Service.Models.VehicleMake, UpdateMakeVM>().ReverseMap();

            CreateMap<Service.Paging<Service.Models.VehicleMake>, MVC.Models.PagedList>()
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.TotalPages,
                    opt => opt.MapFrom(src => (int)Math.Ceiling((double)src.TotalItems / src.PageSize)));


            CreateMap<Paging<Service.Models.VehicleMake>, MakeListVM>()
                .ForMember(dest => dest.Makes, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => src));
        }
    }
}
