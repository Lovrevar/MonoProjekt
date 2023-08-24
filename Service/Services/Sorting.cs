using System.Linq.Expressions;
using System.Reflection;

namespace Service;

public class Sorting<T>
{
    public string SortProperty { get; set; }
    public string SortDirection { get; set; }

    public IQueryable<T> ApplySorting(IQueryable<T> query)
    {
        if (!string.IsNullOrEmpty(SortProperty))
        {
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "e");
            var propertyInfo = entityType.GetProperty(SortProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null)
            {
                var propertyAccess = Expression.Property(parameter, propertyInfo);
                var orderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

                query = SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? Queryable.OrderBy(query, orderByExpression)
                    : Queryable.OrderByDescending(query, orderByExpression);
            }
        }
        
        return query;
    }
}