using Microsoft.EntityFrameworkCore;
using Service.DataAccess;
using Service.Models;

namespace Service;

public class VehicleService : IVehicleService
{
    private readonly MyDbContext _dbContext;

    public VehicleService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<VehicleMake?>> GetVehicleMakesAsync(string searchString)
    {
        var query = _dbContext.VehicleMakes.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(vm =>
                vm.Name.ToLower().Contains(searchString.ToLower()) ||
                vm.Abrv.ToLower().Contains(searchString.ToLower()));
        }

        return await query.ToListAsync();
    }
    public async Task<VehicleMake?> GetVehicleMakeByIdAsync(int id)
    {
        return await _dbContext.VehicleMakes.Include(vm => vm.VehicleModels)
            .FirstOrDefaultAsync(vm => vm.Id == id);
    }
    public async Task<VehicleModel?> GetVehicleModelByIdAsync(int id)
    {
        return await _dbContext.VehicleModels.Include(vmo => vmo.VehicleMake).FirstOrDefaultAsync(vmo => vmo.Id == id);
    }
    public async Task AddVehicleMakeAsync(VehicleMake vehicleMake)
    {
        _dbContext.Set<VehicleMake>().Add(vehicleMake);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateVehicleMakeAsync(VehicleMake? vehicleMake)
    {
        _dbContext.VehicleMakes.Update(vehicleMake);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateVehicleModelAsync(VehicleModel vehicleModel)
    {
        _dbContext.VehicleModels.Update(vehicleModel);

        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteVehicleMakeAsync(int id)
    {
        var vehicleMake = await _dbContext.VehicleMakes.Include(vm => vm.VehicleModels)
            .FirstOrDefaultAsync(vm => vm.Id == id);

        if (vehicleMake?.VehicleModels?.Count > 0)
        {
            throw new Exception("Vehicle Make has Vehicle Models attached");
        }

        if (vehicleMake != null) // Check if vehicleMake is not null before attempting deletion
        {
            _dbContext.VehicleMakes.Remove(vehicleMake);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task DeleteVehicleModelAsync(int id)
    {
        _dbContext.VehicleModels.Remove(new VehicleModel { Id = id });

        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<VehicleMake?>> GetVehicleMakes(int page, int pageSize, string searchString, string sortOrder)
    {
        var query = _dbContext.VehicleMakes.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(vm =>
                vm.Name.ToLower().Contains(searchString.ToLower()) ||
                vm.Abrv.ToLower().Contains(searchString.ToLower()));
        }
        
        query = sortOrder switch
        {
            "id_asc" => query.OrderBy(vm => vm.Id),
            "name_asc" => query.OrderBy(vm => vm.Name),
            "abrv_asc" => query.OrderBy(vm => vm.Abrv),
            _ => query.OrderBy(vm => vm.Id)
        };

        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<List<VehicleMake?>> GetVehicleMakes(string searchString)
    {
        var query = _dbContext.VehicleMakes.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(vm =>
                vm.Name.ToLower().Contains(searchString.ToLower()) ||
                vm.Abrv.ToLower().Contains(searchString.ToLower()));
        }

        return await query.ToListAsync();
    }
    public async Task<List<VehicleModel>> GetVehicleModels(int page, int pageSize, string searchString, string sortOrder)
    {
        var query = _dbContext.VehicleModels.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(vm =>
                vm.Name.ToLower().Contains(searchString.ToLower()) ||
                vm.VehicleMake.Abrv.ToLower().Contains(searchString.ToLower()));
        }
        
        if (!string.IsNullOrEmpty(sortOrder))
        {
            query = sortOrder switch
            {
                "id_asc" => query.OrderBy(vm => vm.Id),
                "name_asc" => query.OrderBy(vm => vm.Name),
                "make_asc" => query.OrderBy(vm => vm.VehicleMake.Name),
                "abrevation_asc" => query.OrderBy(vm => vm.VehicleMake.Abrv),
                _ => query.OrderBy(vm => vm.Id)
            };
        }

        return await query.Skip((page - 1) * pageSize).Take(pageSize).Include(vmo => vmo.VehicleMake).ToListAsync();
    }
    public async Task<List<VehicleModel>> GetVehicleModels(string searchString)
    {
        var query = _dbContext.VehicleModels.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(vm =>
                vm.Name.ToLower().Contains(searchString.ToLower()) ||
                vm.VehicleMake.Abrv.ToLower().Contains(searchString.ToLower()));
        }

        return await query.Include(vmo => vmo.VehicleMake).ToListAsync();
    }

    

    

    
}