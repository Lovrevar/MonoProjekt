using System.Linq.Expressions;

namespace Service;

public class Filtering<T>
{
    public string SearchString { get; set; }

    public IQueryable<T> ApplyFiltering(IQueryable<T> query, Expression<Func<T, bool>> filter)
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            query = query.Where(filter);
        }
        
        return query;
    }
}