using Service.Models;

namespace Service.ModelService;

public interface IVehicleModelService
{
    Task<Paging<VehicleModel>> GetVehicleModels(Filtering<VehicleModel> filteringOptions, Sorting<VehicleModel> sortingOptions, Paging<VehicleModel> pagingOptions);
    Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
    Task AddVehicleModelAsync(VehicleModel vehicleModel);
    Task UpdateVehicleModelAsync(int id, VehicleModel vehicleModel);
    Task DeleteVehicleModelAsync(int id);
}