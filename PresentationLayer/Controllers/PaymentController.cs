using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IUnitOFWork _unitOFWork;


        public PaymentController(UserManager<Employee> userManager, IUnitOFWork unitOFWork)
        {
            _userManager = userManager;
            _unitOFWork = unitOFWork;


        }


        public IActionResult Search()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Search(PaymentSearchViewModel paymentSearchViewModel)
        {
            bool emporder = paymentSearchViewModel.EmployeeOrder == "on" ? true : false;
            var employee = await _userManager.Users
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(e => e.EmpId == paymentSearchViewModel.searchText || e.Email == paymentSearchViewModel.searchText);


            if (employee == null)
            {
                ViewBag.ErrorMessage = "Employee not found.";
                return View();
            }


            List<Order> orders = employee.Orders
                .Where(o => o.IsEmployeeOrder == emporder && o.Status == OrderStatus.Delivered && o.DateTimeOfOrder.Date >= paymentSearchViewModel.fromDate.Date && o.DateTimeOfOrder.Date <= paymentSearchViewModel.toDate.Date).ToList();



            decimal totalAmount = orders.Sum(o => o.Quantity * o.Drink.Price);


            ViewBag.Employee = employee;
            ViewBag.Orders = orders;
            ViewBag.TotalAmount = totalAmount;

            return View("Results");
        }



        public async Task<IActionResult> GenerateInvoicePDF(List<int> orderIds, String employeeId, decimal totalAmount)
        {

            var orders = await _unitOFWork.OrderRepository.GetPdfOrders(orderIds);
            Employee employee = await _userManager.Users.FirstOrDefaultAsync(e => e.Id == employeeId);



            PDFService pDFService = new PDFService();
            var pdfs = pDFService.GeneratePDF(orders.ToList(), employee, totalAmount);
            return File(pdfs, "application/pdf", "receipt.pdf");
        }


    }
}

