using Microsoft.EntityFrameworkCore;
using Service.DataAccess;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Service
{
    public class VehicleService : IVehicleService
    {
        private readonly MyDbContext _dbContext;

        public VehicleService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VehicleMake?>> GetVehicleMakes(QueryParams parameters)
        {
            var query = _dbContext.VehicleMakes.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                query = query.Where(vm =>
                    vm.Name.ToLower().Contains(parameters.SearchString.ToLower()) ||
                    vm.Abrv.ToLower().Contains(parameters.SearchString.ToLower()));
            }

            query = ApplySorting(query, parameters.SortOrder);

            return await ApplyPaging(query, parameters.Page, parameters.PageSize).ToListAsync();
        }

        public async Task<List<VehicleModel>> GetVehicleModels(QueryParams parameters)
        {
            var query = _dbContext.VehicleModels.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                query = query.Where(vm =>
                    vm.Name.ToLower().Contains(parameters.SearchString.ToLower()) ||
                    vm.VehicleMake.Abrv.ToLower().Contains(parameters.SearchString.ToLower()));
            }

            query = ApplySorting(query, parameters.SortOrder);

            return await ApplyPaging(query, parameters.Page, parameters.PageSize).Include(vmo => vmo.VehicleMake).ToListAsync();
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

        
        
        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                return query;
            }

            var sortExpression = sortOrder.Split('_');
            var sortProperty = sortExpression[0];
            var sortDirection = sortExpression[1];

            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "e");
            var propertyInfo = entityType.GetProperty(sortProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                // Handle the case when the property is not found
                return query;
            }

            var propertyAccess = Expression.Property(parameter, propertyInfo);
            var orderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

            var sortedQuery = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? Queryable.OrderBy(query, orderByExpression)
                : Queryable.OrderByDescending(query, orderByExpression);

            return sortedQuery;
        }


        private static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
