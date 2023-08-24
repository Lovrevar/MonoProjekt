using Service.Models;
using MVC.Models.VehicleMake;

namespace Service
{
    public interface IVehicleService
    {
        Task<Paging<VehicleMake?>> GetVehicleMakes(QueryParams parameters);
        Task<MakeListVM> GetMakeListViewModel(QueryParams parameters);
        Task<ModelListVM> GetModelListViewModel(QueryParams parameters);
        Task<Paging<VehicleModel>> GetVehicleModels(QueryParams parameters);
        Task<VehicleMake?> GetVehicleMakeByIdAsync(int id);
        Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
        Task AddVehicleMakeAsync(VehicleMake vehicleMake);
        Task UpdateVehicleMakeAsync(VehicleMake? vehicleMake);
        Task UpdateVehicleModelAsync(VehicleModel vehicleModel);
        Task DeleteVehicleMakeAsync(int id);
        Task DeleteVehicleModelAsync(int id);
    }
}