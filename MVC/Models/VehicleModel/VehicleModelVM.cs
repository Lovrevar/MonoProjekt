namespace MVC.Models.VehicleModel;

public class VehicleModelVM
{
        public List<Service.Models.VehicleModel> VehicleModels { get; set; }
        public int TotalVehicleModels { get; set; }
        public PagedList Pagination { get; set; } 
}