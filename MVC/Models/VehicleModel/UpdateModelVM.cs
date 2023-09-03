using System.ComponentModel.DataAnnotations;

namespace MVC.Models.VehicleModel;

public class UpdateModelVM
{
    public int Id { get; set; }
    [Required]
    public int MakeId { get; set; }
    [Required]
    public string Name { get; set; }
}