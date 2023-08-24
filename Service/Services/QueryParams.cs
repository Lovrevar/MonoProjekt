namespace Service;

public class QueryParams
{
    public string SortProperty;
    public string SortDirection;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string SearchString { get; set; }
    public string SortOrder { get; set; }
}