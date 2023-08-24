using Microsoft.EntityFrameworkCore;
using Service.DataAccess;
using Service.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models.VehicleMake;
using MVC.Models.VehicleModel;

namespace Service
{
    public class VehicleService : IVehicleService
    {
        private readonly MyDbContext _dbContext;

        public VehicleService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MakeListVM> GetMakeListViewModel(QueryParams parameters)
        {
            var pagingResult = await GetVehicleMakes(parameters);

            return new MakeListVM
            {
                Makes = pagingResult.Data,
                Pagination = new PagedList
                {
                    CurrentPage = pagingResult.Page,
                    PageSize = pagingResult.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagingResult.TotalItems / pagingResult.PageSize),
                    SearchString = parameters.SearchString
                }
            };
        }

        public async Task<ModelListVM> GetModelListViewModel(QueryParams parameters)
        {
            var pagingResult = await GetVehicleModels(parameters);

            return new ModelListVM
            {
                Models = pagingResult.Data,
                Pagination = new PagedList
                {
                    CurrentPage = pagingResult.Page,
                    PageSize = pagingResult.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagingResult.TotalItems / pagingResult.PageSize),
                    SearchString = parameters.SearchString
                }
            };
        }

        public async Task<Paging<VehicleMake?>> GetVehicleMakes(QueryParams parameters)
        {
            var query = _dbContext.VehicleMakes.AsQueryable();
            var filteringOptions = new Filtering<VehicleMake?> { SearchString = parameters.SearchString };
            query = filteringOptions.ApplyFiltering(query, vm =>
                vm.Name.ToLower().Contains(parameters.SearchString.ToLower()) ||
                vm.Abrv.ToLower().Contains(parameters.SearchString.ToLower()));

            var sortingOptions = new Sorting<VehicleMake?> { SortProperty = parameters.SortProperty, SortDirection = parameters.SortDirection };
            query = sortingOptions.ApplySorting(query);

            var pagedQuery = ApplyPaging(query, parameters.Page, parameters.PageSize);
            var totalItems = await query.CountAsync();

            return new Paging<VehicleMake?>
            {
                Data = await pagedQuery.ToListAsync(),
                TotalItems = totalItems,
                Page = parameters.Page,
                PageSize = parameters.PageSize
            };
        }

        public async Task<Paging<VehicleModel>> GetVehicleModels(QueryParams parameters)
        {
            var query = _dbContext.VehicleModels.AsQueryable();
            var filteringOptions = new Filtering<VehicleModel> { SearchString = parameters.SearchString };
            query = filteringOptions.ApplyFiltering(query, vm =>
                vm.Name.ToLower().Contains(parameters.SearchString.ToLower()) ||
                vm.VehicleMake.Abrv.ToLower().Contains(parameters.SearchString.ToLower()));

            var sortingOptions = new Sorting<VehicleModel> { SortProperty = parameters.SortProperty, SortDirection = parameters.SortDirection };
            query = sortingOptions.ApplySorting(query);

            var pagedQuery = ApplyPaging(query, parameters.Page, parameters.PageSize).Include(vmo => vmo.VehicleMake);
            var totalItems = await query.CountAsync();

            return new Paging<VehicleModel>
            {
                Data = await pagedQuery.ToListAsync(),
                TotalItems = totalItems,
                Page = parameters.Page,
                PageSize = parameters.PageSize
            };
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

            if (vehicleMake != null)
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

        private IQueryable<T> ApplyPaging<T>(IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
