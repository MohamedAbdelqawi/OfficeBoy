using BusinessLogicLayer;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OfficeBoyController : Controller
    {

        private readonly UserManager<Employee> _userManager;
        private readonly IUnitOFWork _unitOFWork;


        public OfficeBoyController(
        UserManager<Employee> userManager, IUnitOFWork unitOFWork)
        {


            _userManager = userManager;
            _unitOFWork = unitOFWork;


        }








        public async Task<IActionResult> OfficeBoyShift()
        {
            var officeBoys = await _userManager.GetUsersInRoleAsync("OfficeBoy");

            var officeBoyIds = officeBoys.Select(e => e.Id).ToList();
            var shifts = await _unitOFWork.OfficeBoyShiftRepository.GetShiftsByEmployeeIds(officeBoyIds);

            var officeBoysShifts = officeBoys.Select(e => new OfficeBoyShiftViewModel
            {
                EmployeeId = e.Id,
                Employeename = e.Name,
                ShiftStartTime = shifts.FirstOrDefault(s => s.EmployeeId == e.Id)?.ShiftStartTime ?? DateTime.MinValue,
                ShiftEndTime = shifts.FirstOrDefault(s => s.EmployeeId == e.Id)?.ShiftEndTime ?? DateTime.MinValue
            }).ToList();

            return View(officeBoysShifts);
        }

        [HttpGet]
        public async Task<IActionResult> EditShift(string employeeId)
        {
            var existingShift = await _unitOFWork.OfficeBoyShiftRepository.GetShiftById(employeeId);
            if (existingShift != null)
            {
                var employee = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == employeeId);
                var model = new OfficeBoyShiftViewModel
                {
                    EmployeeId = existingShift.EmployeeId,
                    Employeename = employee.Name,
                    ShiftStartTime = existingShift.ShiftStartTime,
                    ShiftEndTime = existingShift.ShiftEndTime
                };
                return View(model);
            }

            return RedirectToAction("AddShift", new { employeeId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(OfficeBoyShiftViewModel model)
        {
            if (ModelState.IsValid)
            {

                var existingShift = await _unitOFWork.OfficeBoyShiftRepository.GetShiftById(model.EmployeeId);

                if (existingShift != null)
                {

                    existingShift.ShiftStartTime = model.ShiftStartTime;
                    existingShift.ShiftEndTime = model.ShiftEndTime;

                    _unitOFWork.OfficeBoyShiftRepository.UpdateShift(existingShift);
                    await _unitOFWork.Save();
                    TempData["UpdateSuccessfully"] = true;

                    return RedirectToAction("OfficeBoyShift");
                }
                else
                {

                    return RedirectToAction("EditShift", new { employeeId = model.EmployeeId });
                }
            }


            return View("EditShift", model);
        }


        [HttpGet]
        public async Task<IActionResult> AddShift(string employeeId)
        {
            var currentuser = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == employeeId);
            ViewBag.currentuser = currentuser;


            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AddShift(OfficeBoyShiftViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingShift = await _unitOFWork.OfficeBoyShiftRepository.GetShiftById(model.EmployeeId);
                if (existingShift != null)
                {

                    ModelState.AddModelError("", "Shift already exists for this employee.");
                    return View(model);
                }

                var officeBoyShift = new OfficeBoyShift()
                {
                    EmployeeId = model.EmployeeId,
                    ShiftStartTime = model.ShiftStartTime,
                    ShiftEndTime = model.ShiftEndTime
                };
                _unitOFWork.OfficeBoyShiftRepository.AddShift(officeBoyShift);
                await _unitOFWork.Save();
                TempData["AddedSuccessfully"] = true;
                return RedirectToAction("OfficeBoyShift");
            }
            return View(model);
        }





    }
}
