using System.Threading.Tasks;
using Service.Models;

namespace Service
{
    public interface IVehicleService
    {
        Task<Paging<VehicleMake?>> GetVehicleMakes(Filtering<VehicleMake?> filteringOptions, Sorting<VehicleMake?> sortingOptions, Paging<VehicleMake?> pagingOptions);
        Task<Paging<VehicleModel>> GetVehicleModels(Filtering<VehicleModel> filteringOptions, Sorting<VehicleModel> sortingOptions, Paging<VehicleModel> pagingOptions);
        Task<VehicleMake?> GetVehicleMakeByIdAsync(int id);
        Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
        Task AddVehicleMakeAsync(VehicleMake vehicleMake);
        Task AddVehicleModelAsync(VehicleModel vehicleModel);
        Task UpdateVehicleMakeAsync(int id, VehicleMake? vehicleMake);
        Task UpdateVehicleModelAsync(int id, VehicleModel vehicleModel);
        Task DeleteVehicleMakeAsync(int id);
        Task DeleteVehicleModelAsync(int id);
    }
}
