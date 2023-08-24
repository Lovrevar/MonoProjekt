namespace Service;

public class Paging<T>
{
    public List<T> Data { get; set; }
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}