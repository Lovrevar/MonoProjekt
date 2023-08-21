namespace MVC.Models.VehicleModel;

public class VehicleModelVM
{
        public List<Service.Models.VehicleModel> VehicleModels { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }

}