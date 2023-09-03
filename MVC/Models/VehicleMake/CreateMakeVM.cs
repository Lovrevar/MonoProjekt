using System.ComponentModel.DataAnnotations;

namespace MVC.Models.VehicleMake;

public class CreateMakeVM
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Abrv { get; set; }
}