namespace MVC.Models.VehicleModel
{
    public class VehicleModelVM
    {
        public Service.Models.VehicleModel VehicleModel;
        public List<Service.Models.VehicleModel> VehicleModels { get; set; }
        public int VehicleMakeId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
    }
}