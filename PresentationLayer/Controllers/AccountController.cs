using BusinessLogicLayer;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IUnitOFWork _unitOFWork;

        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<Employee> signInManager, UserManager<Employee> userManager, IUnitOFWork unitOFWork)
        {

            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOFWork = unitOFWork;
        }


        [HttpGet]

        public async Task<IActionResult> Register()
        {
            var locations = await _unitOFWork.OfficeLocationRepository.GetAllLocations();
            ViewBag.locations = locations;
            return View();
        }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Employee
                {
                    UserName = model.Email,
                    Name = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    OfficeLocation = model.OfficeLocation,
                    EmpId = model.EmployeeId

                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("PlaceOrder", "Order");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }


            }
            return View(model);
        }



        public async Task<IActionResult> Logout()
        {


            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");

        }

        [HttpGet]
        public IActionResult AddRole()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.roles = roles;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {

                var existingRole = await _roleManager.FindByNameAsync(model.RoleName);
                if (existingRole != null)
                {
                    ModelState.AddModelError("", "Role already exists.");
                    return View(model);
                }

                var role = new IdentityRole { Name = model.RoleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["AddedSuccessfully"] = true;
                    return RedirectToAction("AddRole");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return View(model);
        }






        public IActionResult UserRoles()
        {
            var usersWithRoles = _userManager.Users.Select(user => new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList()
            }).ToList();

            return View(usersWithRoles);
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);



            var userRoles = await _userManager.GetRolesAsync(user);


            var allRoles = _roleManager.Roles.Select(role => role.Name).ToList();


            var model = new ManageUserRolesViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles.ToArray());

            if (model.SelectedRoles != null)
            {
                var result = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                if (result.Succeeded)
                {



                    TempData["AddedSuccessfully"] = true;
                    return RedirectToAction("UserRoles");
                }
            }
            else
            {


                TempData["AddedSuccessfully"] = true;
                return RedirectToAction("UserRoles");
            }


            return View(model);
        }



        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);


            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["DeletedSuccessfully"] = true;
                return RedirectToAction("Roles");
            }
            else
            {

                return RedirectToAction("Roles");
            }
        }


    }
}