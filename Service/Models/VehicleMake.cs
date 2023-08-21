using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Service.Models;

[Table("VehicleMake")]
public class VehicleMake
{
    public int Id { get; set; }
    [Required] [StringLength(255)] public string Name { get; set; }
    [Required] [StringLength(255)] public string Abrv { get; set; }

    public List<VehicleModel>? VehicleModels { get; set; } = null;
}