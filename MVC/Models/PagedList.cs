namespace MVC.Models;

public class PagedList
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string SearchString { get; set; }
}