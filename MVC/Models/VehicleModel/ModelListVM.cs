namespace MVC.Models.VehicleModel;

public class ModelListVM
{
    public List<ModelDetailsVM> Models { get; set; }
    public PagedList Pagination { get; set; }
    public string SearchString{ get; set; }
}