using System.ComponentModel.DataAnnotations;

namespace MVC.Models.VehicleMake;

public class MakeDetailsVM
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Abrv { get; set; }
}