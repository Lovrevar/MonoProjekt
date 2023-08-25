using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.VehicleModel;
using Service;
using Service.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleModelController(IVehicleService vehicleService,IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<ViewResult> Index(int page = 1, string searchString = "", string sortOrder = "")
        {
            const int pageSize = 10;

            var filteringOptions = new Filtering<VehicleModel> { SearchString = searchString };
            var sortingOptions = new Sorting<VehicleModel> { SortProperty = "Name", SortDirection = sortOrder };
            var pagingOptions = new Paging<VehicleModel> { Page = page, PageSize = pageSize };

            var pagingResult = await _vehicleService.GetVehicleModels(filteringOptions, sortingOptions, pagingOptions);

            var viewModel = new ModelListVM
            {
                Models = _mapper.Map<List<ModelDetailsVM>>(pagingResult.Data),
                Pagination = new PagedList
                {
                    CurrentPage = pagingResult.Page,
                    PageSize = pagingResult.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagingResult.TotalItems / pagingResult.PageSize)
                },
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
public async Task<IActionResult> Create()
{
    var filteringOptions = new Filtering<VehicleMake?>();
    var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
    var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

    var vehicleMakes = await _vehicleService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
    ViewBag.vehicleMakes = vehicleMakes.Data;

    return View();
}

[HttpPost]
public async Task<IActionResult> Create(CreateModelVM createModelVM)
{
    if (!ModelState.IsValid)
    {
        return View(createModelVM);
    }

    var vehicleModel = _mapper.Map<VehicleModel>(createModelVM);

    try
    {
        await _vehicleService.AddVehicleModelAsync(vehicleModel); 
        return RedirectToAction("Index");
    }
    catch (Exception)
    {
        ModelState.AddModelError("", "Failed to write to the database.");

        // Repopulate the ViewBag data here
        var filteringOptions = new Filtering<VehicleMake?>();
        var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
        var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

        var vehicleMakes = await _vehicleService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
        ViewBag.vehicleMakes = vehicleMakes.Data;

        return View(createModelVM);
    }
}

[HttpGet]
public async Task<IActionResult> Edit(int id)
{
    var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(id);

    if (vehicleModel == null)
    {
        return NotFound();
    }

    var filteringOptions = new Filtering<VehicleMake?>();
    var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
    var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

    var vehicleMakes = await _vehicleService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
    ViewBag.vehicleMakes = vehicleMakes.Data;

    return View(_mapper.Map<UpdateModelVM>(vehicleModel));
}

[HttpPost]
public async Task<IActionResult> Edit(int id, UpdateModelVM updateModelVM)
{
    if (!ModelState.IsValid)
    {
        return View(updateModelVM);
    }

    var vehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(updateModelVM.Id);

    if (vehicleMake == null)
    {
        ModelState.AddModelError("VehicleMakeId", "Vehicle make does not exist.");
        var filteringOptions = new Filtering<VehicleMake?>();
        var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
        var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

        var vehicleMakes = await _vehicleService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
        ViewBag.vehicleMakes = vehicleMakes.Data;

        return View(updateModelVM);
    }

    var vehicleModel = _mapper.Map<VehicleModel>(updateModelVM);
    vehicleModel.VehicleMake = vehicleMake;
    vehicleModel.Abrv = vehicleMake.Abrv;

    ModelState.Clear();

    if (!TryValidateModel(vehicleModel, nameof(vehicleModel)))
    {
        var filteringOptions = new Filtering<VehicleMake?>();
        var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
        var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

        var vehicleMakes = await _vehicleService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
        ViewBag.vehicleMakes = vehicleMakes.Data;

        return View(updateModelVM);
    }

    await _vehicleService.UpdateVehicleModelAsync(id, vehicleModel); // Use the correct method here

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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteVehicleModelAsync(id);

            return RedirectToAction("Index");
        }
    }
}
