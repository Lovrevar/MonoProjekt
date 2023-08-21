using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Models;

[Table("VehicleModel")]
    public class VehicleModel
    {
        public int Id { get; set; }
        [Required] public int VehicleMakeId { get; set; }
        [Required] [StringLength(255)] public string Name { get; set; }
        [Required] [StringLength(255)] public string Abrv { get; set; }

        [Required] public VehicleMake? VehicleMake { get; set; }
    }