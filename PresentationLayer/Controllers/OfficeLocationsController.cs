using BusinessLogicLayer;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
[Authorize(Roles = "Admin")]
public class OfficeLocationsController : Controller
{


    private readonly UserManager<Employee> _userManager;
    private readonly IUnitOFWork _unitOFWork;


    public OfficeLocationsController(
    UserManager<Employee> userManager, IUnitOFWork unitOFWork)
    {


        _userManager = userManager;
        _unitOFWork = unitOFWork;


    }


    public async Task<IActionResult> OfficeLocation(int? id)
    {

        var allLocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();
        ViewBag.AllLocations = allLocations;

        OfficeLocationViewModel model;
        if (id.HasValue)
        {
            var location = await _unitOFWork.OfficeLocationRepository.GetLocationById(id.Value);
            if (location == null)
            {
                return NotFound();
            }
            model = new OfficeLocationViewModel
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address
            };
        }
        else
        {
            model = new OfficeLocationViewModel();
        }

        return View(model);
    }

    public async Task<IActionResult> EditOfficeLocation(int? id)
    {
        if (id > 0)
        {

            var location = await _unitOFWork.OfficeLocationRepository.GetLocationById(id.Value);
            var model = new OfficeLocationViewModel
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address
            };
            return View(model);

        }
        else
        {
            var model = new OfficeLocationViewModel();

            return View(model);
        }

    }
    [HttpPost]
    public async Task<IActionResult> EditOfficeLocation(OfficeLocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var location = await _unitOFWork.OfficeLocationRepository.GetLocationById(model.Id);


        location.Name = model.Name;
        location.Address = model.Address;
        _unitOFWork.OfficeLocationRepository.UpdateLocation(location);
        await _unitOFWork.Save();
        TempData["UpdateSuccessfully"] = true;
        return RedirectToAction("OfficeLocation");
    }
    public async Task<IActionResult> AddOffice(int? id)
    {

        var model = new OfficeLocationViewModel();

        return View(model);

    }



    [HttpPost]
    public async Task<IActionResult> AddOfficeLocation(OfficeLocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var newLocation = new OfficeLocation
        {
            Name = model.Name,
            Address = model.Address
        };
        _unitOFWork.OfficeLocationRepository.AddLocation(newLocation);
        await _unitOFWork.Save();
        TempData["AddedSuccessfully"] = true;
        return RedirectToAction("OfficeLocation");
    }



    public async Task<IActionResult> DeleteOfficeLocation(int id)
    {


        _unitOFWork.OfficeLocationRepository.DeleteLocation(id);
        await _unitOFWork.Save();
        TempData["DeletedSuccessfully"] = true;
        return RedirectToAction("OfficeLocation");
    }




}
