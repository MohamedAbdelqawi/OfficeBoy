using BusinessLogicLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin,OfficeBoy")]
    public class DashboardController : Controller
    {

        private readonly IUnitOFWork _unitOFWork;

        public DashboardController(IUnitOFWork unitOFWork)
        {

            _unitOFWork = unitOFWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> orders = await _unitOFWork.OrderRepository.GetAllOrders();
            var officelocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();
            ViewBag.officelocations = officelocations;
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = await _unitOFWork.OrderRepository.GetOrderById(orderId);


            order.Status = newStatus;


            _unitOFWork.OrderRepository.UpdateOrderStatus(order);
            await _unitOFWork.Save();
            TempData["UpdateSuccessfully"] = true;

            return RedirectToAction(nameof(Index));
        }
    }
}
