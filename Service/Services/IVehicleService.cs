using Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IVehicleService
    {
        Task<List<VehicleMake?>> GetVehicleMakes(QueryParams parameters);
        Task<List<VehicleModel>> GetVehicleModels(QueryParams parameters);
        Task<VehicleMake?> GetVehicleMakeByIdAsync(int id);
        Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
        Task AddVehicleMakeAsync(VehicleMake vehicleMake);
        Task UpdateVehicleMakeAsync(VehicleMake? vehicleMake);
        Task UpdateVehicleModelAsync(VehicleModel vehicleModel);
        Task DeleteVehicleMakeAsync(int id);
        Task DeleteVehicleModelAsync(int id);
    }
}