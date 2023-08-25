using AutoMapper;
using MVC.Models.VehicleMake;
using Service;
using Service.Models;

namespace MVC.MappingProfiles
{
    public class VehicleMakeProfile : Profile
    {
        public VehicleMakeProfile()
        {
            CreateMap<VehicleMake, MakeDetailsVM>();
            CreateMap<VehicleMake, CreateMakeVM>();
            CreateMap<VehicleMake, UpdateMakeVM>();

            CreateMap<Service.Paging<VehicleMake>, MVC.Models.PagedList>()
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.TotalPages,
                    opt => opt.MapFrom(src => (int)Math.Ceiling((double)src.TotalItems / src.PageSize)));


            CreateMap<Paging<VehicleMake>, MakeListVM>()
                .ForMember(dest => dest.Makes, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => src));
        }
    }
}
