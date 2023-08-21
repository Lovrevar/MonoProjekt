using Service.Models;

namespace Service;

public interface IVehicleService
{
    Task<List<VehicleMake?>> GetVehicleMakesAsync(string searchString);
    Task<VehicleMake?> GetVehicleMakeByIdAsync(int id);
    Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
    Task AddVehicleMakeAsync(VehicleMake vehicleMake);
    Task UpdateVehicleMakeAsync(VehicleMake? vehicleMake);
    Task UpdateVehicleModelAsync(VehicleModel vehicleModel);
    Task DeleteVehicleMakeAsync(int id);
    Task DeleteVehicleModelAsync(int id);
    Task<List<VehicleMake?>> GetVehicleMakes(int page, int pageSize, string searchString, string sortOrder);
    Task<List<VehicleMake?>> GetVehicleMakes(string searchString);

    Task<List<VehicleModel>> GetVehicleModels(int page, int pageSize, string searchString, string sortOrder);
    Task<List<VehicleModel>> GetVehicleModels(string searchString);

 

}