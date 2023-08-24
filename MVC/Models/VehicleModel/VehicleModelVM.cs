using System.Collections.Generic;
using Service.Models; 
using MVC.Models; 

namespace MVC.Models.VehicleModel
{
    public class VehicleModelVM
    {
        public List<Service.Models.VehicleModel> Models { get; set; } // Assuming VehicleModel is from the Service.Models namespace
        public int TotalVehicleModels { get; set; }
        public PagedList Pagination { get; set; } // Assuming PagedList is from the MVC.Models namespace
    }
}