using Microsoft.EntityFrameworkCore;
using Service.DataAccess;
using Service.Models;

namespace Service.Services.MakeService
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly MyDbContext _dbContext;

        public VehicleMakeService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Paging<VehicleMake?>> GetVehicleMakes(Filtering<VehicleMake?> filteringOptions, Sorting<VehicleMake?> sortingOptions, Paging<VehicleMake?> pagingOptions)
        {
            var query = _dbContext.VehicleMakes.AsQueryable();
            query = filteringOptions.ApplyFiltering(query, vm =>
                vm.Name.ToLower().Contains(filteringOptions.SearchString.ToLower()) ||
                vm.Abrv.ToLower().Contains(filteringOptions.SearchString.ToLower()));

            query = sortingOptions.ApplySorting(query);

            var pagedQuery = ApplyPaging(query, pagingOptions.Page, pagingOptions.PageSize);
            var totalItems = await query.CountAsync();

            return new Paging<VehicleMake?>
            {
                Data = await pagedQuery.ToListAsync(),
                TotalItems = totalItems,
                Page = pagingOptions.Page,
                PageSize = pagingOptions.PageSize
            };
        }

        public async Task<VehicleMake?> GetVehicleMakeByIdAsync(int id)
        {
            return await _dbContext.VehicleMakes.Include(vm => vm.VehicleModels)
                .FirstOrDefaultAsync(vm => vm.Id == id);
        }

        public async Task AddVehicleMakeAsync(VehicleMake vehicleMake)
        {
            _dbContext.Set<VehicleMake>().Add(vehicleMake);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateVehicleMakeAsync(int id, VehicleMake updatedVehicleMake)
        {
            var existingVehicleMake = await _dbContext.VehicleMakes.FindAsync(id);

            if (existingVehicleMake == null)
            {
                throw new ArgumentException("Vehicle Make with the specified id not found.", nameof(id));
            }

            // Update the properties of the existingVehicleMake with the properties from updatedVehicleMake
            existingVehicleMake.Name = updatedVehicleMake.Name;
            existingVehicleMake.Abrv = updatedVehicleMake.Abrv;

            _dbContext.VehicleMakes.Update(existingVehicleMake);
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

            if (vehicleMake != null)
            {
                _dbContext.VehicleMakes.Remove(vehicleMake);
                await _dbContext.SaveChangesAsync();
            }
        }
        private IQueryable<T> ApplyPaging<T>(IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public string GetAbrvForMakeById(int makeId)
        {
            // Assuming you have a method to retrieve a VehicleMake by Id from your data source
            var vehicleMake = _dbContext.VehicleMakes.FirstOrDefault(vm => vm.Id == makeId);
            return vehicleMake.Abrv;
        }

    }
}
