using Microsoft.AspNetCore.Mvc;
using MVC.Models.VehicleMake;
using Service;
using System;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using Service.Models;

namespace MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleMakeController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index(int page = 1, string searchString = "", string sortOrder = "")
        {
            const int pageSize = 10;

            var vehicleMakes = await _vehicleService.GetVehicleMakes(new QueryParams()
            {
                Page = page,
                PageSize = pageSize,
                SearchString = searchString,
                SortOrder = sortOrder
            });

            var allVehicleMakes = await _vehicleService.GetVehicleMakes(new QueryParams()
            {
                SearchString = searchString
            });
            var totalPages = (int)Math.Ceiling((double)allVehicleMakes.Count() / pageSize);

            page = Math.Clamp(page, 0, totalPages);

            var viewModel = new VehicleMakeVM()
            {
                VehicleMakes = vehicleMakes,
                TotalVehicleMakes = allVehicleMakes.Count(),
                Pagination = new PagedList
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    SearchString = searchString
                }
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleMake vehicleMake)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicleMake);
            }

            try
            {
                await _vehicleService.AddVehicleMakeAsync(vehicleMake);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to write to the database.");
                return View(vehicleMake);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            return View(vehicleMake);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMake vehicleMake)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicleMake);
            }

            await _vehicleService.UpdateVehicleMakeAsync(vehicleMake);

            return RedirectToAction("Index");
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
