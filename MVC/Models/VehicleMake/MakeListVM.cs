namespace MVC.Models.VehicleMake;

public class MakeListVM
{
    public List<MakeDetailsVM> Makes { get; set; }
    public PagedList Pagination { get; set; }
    public string SearchString{ get; set; }
}