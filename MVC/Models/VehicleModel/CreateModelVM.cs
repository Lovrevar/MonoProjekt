using System.ComponentModel.DataAnnotations;

namespace MVC.Models.VehicleModel;

public class CreateModelVm
{
    [Required]
    public int MakeId { get; set; }
    
    [Required]
    public string Name { get; set; }
}