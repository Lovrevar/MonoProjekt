using Service.Models;

namespace Service.Services.MakeService;

public interface IVehicleMakeService
{
    Task<Paging<VehicleMake?>> GetVehicleMakes(Filtering<VehicleMake?> filteringOptions,
        Sorting<VehicleMake?> sortingOptions, Paging<VehicleMake?> pagingOptions);
    Task<VehicleMake?> GetVehicleMakeByIdAsync(int id);
    Task AddVehicleMakeAsync(VehicleMake vehicleMake);
    Task UpdateVehicleMakeAsync(int id, VehicleMake? vehicleMake);
    Task DeleteVehicleMakeAsync(int id);
}