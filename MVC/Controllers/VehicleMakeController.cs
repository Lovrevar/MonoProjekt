using Microsoft.AspNetCore.Mvc;
using MVC.Models.VehicleMake;
using Service;
using System;
using System.Threading.Tasks;
using AutoMapper;
using MVC.Models;
using Service.Models;

namespace MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleMakeController(IVehicleService vehicleService,IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1, string searchString = "", string sortOrder = "")
        {
            const int pageSize = 10;

            var filteringOptions = new Filtering<VehicleMake?> { SearchString = searchString };
            var sortingOptions = new Sorting<VehicleMake?> { SortProperty = "Name", SortDirection = sortOrder };
            var pagingOptions = new Paging<VehicleMake?> { Page = page, PageSize = pageSize };

            var pagingResult = await _vehicleService.GetVehicleMakes(filteringOptions, sortingOptions, pagingOptions);

            var viewModel = _mapper.Map<MakeListVM>(pagingResult);
            viewModel.SearchString = searchString;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMakeVM createMakeVm)
        {
            if (!ModelState.IsValid)
            {
                return View(createMakeVm);
            }

            var vehicleMake = _mapper.Map<VehicleMake>(createMakeVm);

            try
            {
                await _vehicleService.AddVehicleMakeAsync(vehicleMake);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to write to the database.");
                return View(createMakeVm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleMakeWithModels = await _vehicleService.GetVehicleMakeByIdAsync(id);

            if (vehicleMakeWithModels == null)
            {
                return NotFound();
            }

            var updateMakeVM = _mapper.Map<UpdateMakeVM>(vehicleMakeWithModels);

            // No need to fetch related data separately since it's already included

            return View(updateMakeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            return View(vehicleMake);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            try
            {
                await _vehicleService.DeleteVehicleMakeAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id, errorMessage = "Failed to delete Vehicle Make" });
            }
        }
    }
}
