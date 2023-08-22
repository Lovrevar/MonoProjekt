namespace MVC.Models.VehicleMake;

public class VehicleMakeVM
{
    public List<Service.Models.VehicleMake> VehicleMakes { get; set; }
    public Service.Models.VehicleMake VehicleMake { get; set; }
    public int TotalVehicleMakes { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string SearchString { get; set; }
}