using System.ComponentModel.DataAnnotations;

namespace MVC.Models.VehicleModel;

public class UpdateModelVm
{
    public int Id { get; set; }
    [Required]
    public int VehicleMakeId { get; set; }
    [Required]
    public string Name { get; set; }
}