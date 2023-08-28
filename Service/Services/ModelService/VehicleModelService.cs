using Microsoft.EntityFrameworkCore;
using Service.DataAccess;
using Service.Models;
using Service.ModelService;

namespace Service.Services.ModelService
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly MyDbContext _dbContext;

        public VehicleModelService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Paging<VehicleModel>> GetVehicleModels(Filtering<VehicleModel> filteringOptions, Sorting<VehicleModel> sortingOptions, Paging<VehicleModel> pagingOptions)
        {
            var query = _dbContext.VehicleModels.AsQueryable();
            query = filteringOptions.ApplyFiltering(query, vm =>
                vm.Name.ToLower().Contains(filteringOptions.SearchString.ToLower()) ||
                vm.VehicleMake.Abrv.ToLower().Contains(filteringOptions.SearchString.ToLower()));

            query = sortingOptions.ApplySorting(query);

            var pagedQuery = ApplyPaging(query, pagingOptions.Page, pagingOptions.PageSize).Include(vmo => vmo.VehicleMake);
            var totalItems = await query.CountAsync();

            return new Paging<VehicleModel>
            {
                Data = await pagedQuery.ToListAsync(),
                TotalItems = totalItems,
                Page = pagingOptions.Page,
                PageSize = pagingOptions.PageSize
            };
        }

        public async Task<VehicleModel?> GetVehicleModelByIdAsync(int id)
        {
            return await _dbContext.VehicleModels.Include(vmo => vmo.VehicleMake).FirstOrDefaultAsync(vmo => vmo.Id == id);
        }

        public async Task AddVehicleModelAsync(VehicleModel vehicleModel)
        {
            _dbContext.VehicleModels.Add(vehicleModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateVehicleModelAsync(int id, VehicleModel vehicleModel)
        {
            var existingModel = await _dbContext.VehicleModels.FirstOrDefaultAsync(vm => vm.Id == id);

            if (existingModel == null)
            {
                throw new ArgumentException("Vehicle model not found.", nameof(id));
            }

            // Update properties of the existingModel based on the vehicleModel parameter
            existingModel.Name = vehicleModel.Name;
            existingModel.VehicleMakeId = vehicleModel.VehicleMakeId;

            _dbContext.VehicleModels.Update(existingModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelAsync(int id)
        {
            _dbContext.VehicleModels.Remove(new VehicleModel { Id = id });
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> ApplyPaging<T>(IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}