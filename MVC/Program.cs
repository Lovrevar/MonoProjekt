using Microsoft.AspNetCore.Routing.Constraints;
using Service.DataAccess;
using Service.Services.MakeService;
using Service.ModelService;
using Service.Services.ModelService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddScoped<IVehicleMakeService, VehicleMakeService>();
builder.Services.AddScoped<IVehicleModelService, VehicleModelService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "vehicleMake",
    pattern: "{controller=VehicleMake}/{action=Delete}/{id?}",
    defaults: new { action = "Delete" },
    constraints: new { httpMethod = new HttpMethodRouteConstraint("DELETE") });
app.MapControllerRoute(
    name: "vehicleModel",
    pattern: "{controller=VehicleModel}/{action=IndexModel}/{id?}");
   

app.Run();