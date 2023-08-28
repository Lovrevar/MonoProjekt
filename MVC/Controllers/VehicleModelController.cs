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
using Service.ModelService;
using Service.Services.MakeService;

namespace MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IVehicleMakeService _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleModelController(IVehicleModelService vehicleModelService,IVehicleMakeService vehicleMakeService, IMapper mapper)
        {
            _vehicleModelService = vehicleModelService;
            _vehicleMakeService = vehicleMakeService;
            _mapper = mapper;
        }

        public async Task<ViewResult> Index(int page = 1, string searchString = "", string sortOrder = "")
        {
            const int pageSize = 10;

            var filteringOptions = new Filtering<VehicleModel> { SearchString = searchString };
            var sortingOptions = new Sorting<VehicleModel> { SortProperty = "Name", SortDirection = sortOrder };
            var pagingOptions = new Paging<VehicleModel> { Page = page, PageSize = pageSize };

            var pagingResult = await _vehicleModelService.GetVehicleModels(filteringOptions, sortingOptions, pagingOptions);

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

    var vehicleMakes = await _vehicleMakeService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
    ViewBag.vehicleMakes = vehicleMakes.Data;

    return View();
}

[HttpPost]
public async Task<IActionResult> Create(CreateModelVM createModelVm)
{
    if (!ModelState.IsValid)
    {
        return View(createModelVm);
    }

    var vehicleModel = _mapper.Map<VehicleModel>(createModelVm);

    try
    {
        await _vehicleModelService.AddVehicleModelAsync(vehicleModel); 
        return RedirectToAction("Index");
    }
    catch (Exception)
    {
        return View(createModelVm);
    }
}

[HttpGet]
public async Task<IActionResult> Edit(int id)
{
    var vehicleModel = await _vehicleModelService.GetVehicleModelByIdAsync(id);

    if (vehicleModel == null)
    {
        return NotFound();
    }

    var filteringOptions = new Filtering<VehicleMake?>();
    var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
    var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

    var vehicleMakes = await _vehicleMakeService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
    ViewBag.vehicleMakes = vehicleMakes.Data;

    return View(_mapper.Map<UpdateModelVM>(vehicleModel));
}

[HttpPost]
public async Task<IActionResult> Edit(int id, UpdateModelVM updateModelVm)
{
    if (!ModelState.IsValid)
    {
        return View(updateModelVm);
    }

    var vehicleMake = await _vehicleMakeService.GetVehicleMakeByIdAsync(updateModelVm.Id);

    if (vehicleMake == null)
    {
        ModelState.AddModelError("VehicleMakeId", "Vehicle make does not exist.");
        var filteringOptions = new Filtering<VehicleMake?>();
        var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
        var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

        var vehicleMakes = await _vehicleMakeService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
        ViewBag.vehicleMakes = vehicleMakes.Data;

        return View(updateModelVm);
    }

    var vehicleModel = _mapper.Map<VehicleModel>(updateModelVm);
    vehicleModel.VehicleMake = vehicleMake;
    vehicleModel.Abrv = vehicleMake.Abrv;

    ModelState.Clear();

    if (!TryValidateModel(vehicleModel, nameof(vehicleModel)))
    {
        var filteringOptions = new Filtering<VehicleMake?>();
        var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = "asc" };
        var pagingOptions = new Paging<VehicleMake?> { Page = 1, PageSize = int.MaxValue };

        var vehicleMakes = await _vehicleMakeService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);
        ViewBag.vehicleMakes = vehicleMakes.Data;

        return View(updateModelVm);
    }

    await _vehicleModelService.UpdateVehicleModelAsync(id, vehicleModel); 

    return RedirectToAction("Index");
}

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleModel = await _vehicleModelService.GetVehicleModelByIdAsync(id);

            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleModelService.DeleteVehicleModelAsync(id);

            return RedirectToAction("Index");
        }
    }
}
