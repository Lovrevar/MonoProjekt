using Microsoft.EntityFrameworkCore;
using Service.Models;

namespace Service.DataAccess;

public class MyDbContext : DbContext
{
    public DbSet<VehicleMake?> VehicleMakes { get; set; }
    public DbSet<VehicleModel> VehicleModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("Data Source = Database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleModel>(vehicleModel =>
        {
            vehicleModel.Property(vmo => vmo.Name).IsRequired().HasMaxLength(255);
            vehicleModel.Property(vmo => vmo.Abrv).IsRequired().HasMaxLength(255);
            
            vehicleModel.HasOne(vmo => vmo.VehicleMake)
                .WithMany(vmo => vmo.VehicleModels)
                .HasForeignKey(vm => vm.VehicleMakeId);
        });

        modelBuilder.Entity<VehicleMake>(vehicleMake =>
        {
            vehicleMake.HasKey(vma => vma.Id);
            vehicleMake.Property(vma => vma.Name).IsRequired().HasMaxLength(255);
            vehicleMake.Property(vma => vma.Abrv).IsRequired().HasMaxLength(255);

            vehicleMake.HasMany<VehicleModel>(vm => vm.VehicleModels)
                .WithOne(vm => vm.VehicleMake);
        });
    }
}