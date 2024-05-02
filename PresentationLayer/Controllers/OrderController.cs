using BusinessLogicLayer;
using DataLayer.Entities;

using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;


        public OrderController(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;

        }



        public async Task<IActionResult> PlaceOrder(string? search)
        {

            var drinks = await _unitOFWork.DrinkRepository.GetAllAvailableDrinks(search);
            var officeLocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();


            ViewBag.Drinks = drinks;
            ViewBag.OfficeLocations = officeLocations;

            var orderViewModel = new OrderViewModel();
            return View(orderViewModel);
        }


        public async Task<IActionResult> SearchDrinks(string search)
        {
            var drinks = await _unitOFWork.DrinkRepository.GetAllAvailableDrinks(search);
            var officeLocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();

            ViewBag.OfficeLocations = officeLocations;

            return PartialView("_DrinkListPartial", drinks); // Pass the drinks model to the partial view
        }




        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderViewModel orderViewModel)
        {

            if (!ModelState.IsValid)
            {

                var drinks = await _unitOFWork.DrinkRepository.GetAllAvailableDrinks(null);
                var officeLocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();
                ViewBag.Drinks = drinks;
                ViewBag.OfficeLocations = officeLocations;

                ModelState.AddModelError("OfficeId", "Please select  location.");

                return View(orderViewModel);
            }


            var officeLocation = await _unitOFWork.OfficeLocationRepository.GetLocationById(orderViewModel.OfficeId);
            if (officeLocation == null)
            {
                var drink = await _unitOFWork.DrinkRepository.GetDrinkById(orderViewModel.DrinkId);

                if (orderViewModel.DrinkId == drink.Id)
                {
                    ModelState.AddModelError($"OfficeId_{drink.Id}", "Please select  location.");
                }




                var drinks = await _unitOFWork.DrinkRepository.GetAllAvailableDrinks(null);
                var officeLocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();
                ViewBag.Drinks = drinks;
                ViewBag.OfficeLocations = officeLocations;

                return View(orderViewModel);
            }


            var order = new Order
            {
                DrinkId = orderViewModel.DrinkId,
                Quantity = orderViewModel.Quantity,
                OfficeId = orderViewModel.OfficeId,
                IsEmployeeOrder = orderViewModel.IsEmployeeOrder,
                EmployeeId = orderViewModel.Id,
                Status = OrderStatus.Received
            };


            _unitOFWork.OrderRepository.AddOrder(order);
            await _unitOFWork.Save();


            TempData["OrderPlacedSuccessfully"] = true;
            return RedirectToAction("PlaceOrder");
        }

        public async Task<IActionResult> Yourorders()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            IEnumerable<Order> orders = await _unitOFWork.OrderRepository.GetAllOrdersForUser(userId);


            var officelocations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();
            ViewBag.officelocations = officelocations;

            return View(orders);
        }


        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _unitOFWork.OrderRepository.GetOrderById(orderId);
            order.Status = OrderStatus.cancel;
            _unitOFWork.OrderRepository.UpdateOrderStatus(order);

            await _unitOFWork.Save();
            TempData["CancelSuccessfully"] = true;
            return RedirectToAction("Yourorders");
        }



        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
