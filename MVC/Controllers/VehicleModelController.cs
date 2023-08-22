using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.VehicleModel;
using Service;
using Service.Models;

namespace MVC.Controllers;

public class VehicleModelController : Controller
{
    private readonly IVehicleService _vehicleService;
    private readonly IMapper _mapper;
    public VehicleModelController(IVehicleService vehicleService, IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    public async Task<ViewResult> Index(int page = 0, string searchString = "", string sortOrder = "")
    {
        const int pageSize = 10;

        var vehicleModels = await _vehicleService.GetVehicleModels(page, pageSize, searchString, sortOrder);

        var allVehicleModels = await _vehicleService.GetVehicleModels(searchString);
        var totalPages = (int)Math.Ceiling((double)allVehicleModels.Count() / pageSize);

        page = Math.Clamp(page, 0, totalPages);

        var viewModel = new VehicleModelVM()
        {
            VehicleModels = vehicleModels,
            CurrentPage = page,
            SearchString = searchString,
            TotalPages = totalPages,
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var vehicleMakes = await _vehicleService.GetVehicleMakesAsync("");

        ViewBag.vehicleMakes = vehicleMakes;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleModelVM viewModel)
    {
        if (!ModelState.IsValid)
        {
            var vehicleMakes = await _vehicleService.GetVehicleMakesAsync("");
            ViewBag.vehicleMakes = vehicleMakes;
            return View(viewModel);
        }

        var domainModel = _mapper.Map<Service.Models.VehicleModel>(viewModel);

        var vehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(viewModel.VehicleMakeId);
        if (vehicleMake == null)
        {
            ModelState.AddModelError("VehicleMakeId", "Vehicle make does not exist.");
            var vehicleMakes = await _vehicleService.GetVehicleMakesAsync("");
            ViewBag.vehicleMakes = vehicleMakes;
            return View(viewModel);
        }

        domainModel.VehicleMake = vehicleMake;
        domainModel.Abrv = vehicleMake.Abrv;

        await _vehicleService.UpdateVehicleModelAsync(domainModel);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(id);

        if (vehicleModel == null)
        {
            return NotFound();
        }

        var vehicleMakes = await _vehicleService.GetVehicleMakesAsync("");

        var viewModel = _mapper.Map<VehicleModelVM>(vehicleModel); // Use AutoMapper to map the domain model

        viewModel.VehicleModel = vehicleModel; // Assign the domain model to the VehicleModel property

        ViewBag.vehicleMakes = vehicleMakes;

        return View(viewModel); // Pass the VehicleModelVM to the view
    }

    [HttpPost]
    public async Task<IActionResult> Edit(VehicleModelVM viewModel)
    {
        if (!ModelState.IsValid)
        {
            var vehicleMakes = await _vehicleService.GetVehicleMakesAsync("");
            ViewBag.vehicleMakes = vehicleMakes;
            return View(viewModel);
        }

        var domainModel = _mapper.Map<Service.Models.VehicleModel>(viewModel);

        var vehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(viewModel.VehicleMakeId);
        if (vehicleMake == null)
        {
            ModelState.AddModelError("VehicleMakeId", "Vehicle make does not exist.");
            var vehicleMakes = await _vehicleService.GetVehicleMakesAsync("");
            ViewBag.vehicleMakes = vehicleMakes;
            return View(viewModel);
        }

        domainModel.VehicleMake = vehicleMake;
        domainModel.Abrv = vehicleMake.Abrv;

        await _vehicleService.UpdateVehicleModelAsync(domainModel);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(id);

        if (vehicleModel == null)
        {
            return NotFound();
        }

        return View(vehicleModel);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
    {
        await _vehicleService.DeleteVehicleModelAsync(id);

        return RedirectToAction("Index");
    }
}