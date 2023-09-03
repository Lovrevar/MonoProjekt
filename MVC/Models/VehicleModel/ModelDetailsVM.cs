using System.ComponentModel.DataAnnotations;

namespace MVC.Models.VehicleModel;

public class ModelDetailsVM
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string MakeName { get; set; }
    [Required]
    public string MakeAbrv { get; set; }
}