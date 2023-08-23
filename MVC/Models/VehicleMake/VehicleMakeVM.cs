namespace MVC.Models.VehicleMake;

public class VehicleMakeVM
{
    public List<Service.Models.VehicleMake> VehicleMakes { get; set; }
    public int TotalVehicleMakes { get; set; }
    
    public PagedList Pagination { get; set; } 
}