using BusinessLogicLayer;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DrinkController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public DrinkController(IWebHostEnvironment webHostEnvironment, IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
            _webHostEnvironment = webHostEnvironment;
        }




        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 5;

            var query = await _unitOFWork.DrinkRepository.GetAllDrinks(null);


            var drinks = query.Select(d => new DrinkViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                PictureUrl = d.PictureUrl,
                Availability = d.Availability,
                TimeToPrepareMinutes = d.TimeToPrepareMinutes
            })
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();


            int totalDrinks = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalDrinks / pageSize);
            ViewBag.PageNumber = pageNumber;
            ViewBag.IsMainPage = true;
            return View(drinks);
        }

        public async Task<IActionResult> Search(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {

                return RedirectToAction("Index");
            }

            int pageSize = 5;

            var query = await _unitOFWork.DrinkRepository.GetAllDrinks(searchTerm);



            var drinks = query.Select(d => new DrinkViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                PictureUrl = d.PictureUrl,
                Availability = d.Availability,
                TimeToPrepareMinutes = d.TimeToPrepareMinutes
            })
                            .Take(pageSize)
                            .ToList();


            int totalDrinks = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalDrinks / pageSize);
            ViewBag.PageNumber = 1;


            ViewBag.SearchTerm = searchTerm;

            return View("Index", drinks);
        }



        public IActionResult Add()
        {
            var model = new DrinkViewModel();
            return View("Edit");
        }



        [HttpPost]
        public async Task<IActionResult> AddEditDrink(DrinkViewModel model)
        {

            if (!ModelState.IsValid)
                return View("Edit", model);

            string uniqueFileName = null;
            if (model.PictureFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PictureFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PictureFile.CopyTo(fileStream);
                }
            }
            else if (model.Id == null)
            {

                ModelState.AddModelError(nameof(model.PictureFile), "The PictureFile field is required.");
                return View("Edit", model);
            }

            if (model.Id.HasValue && model.Id > 0)
            {

                var drink = await _unitOFWork.DrinkRepository.GetDrinkById(model.Id.Value);
                if (drink == null)
                {
                    return NotFound();
                }

                drink.Name = model.Name;
                drink.Description = model.Description;
                drink.Price = model.Price;
                drink.Availability = model.Availability;
                drink.TimeToPrepareMinutes = model.TimeToPrepareMinutes;


                if (uniqueFileName != null)
                {

                    if (!string.IsNullOrEmpty(drink.PictureUrl))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, drink.PictureUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    drink.PictureUrl = Path.Combine("Images", uniqueFileName);
                }

                _unitOFWork.DrinkRepository.UpdateDrink(drink);
                _unitOFWork.Save();
                TempData["UpdateSuccessfully"] = true;

            }
            else
            {

                var newDrink = new Drink
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    PictureUrl = uniqueFileName != null ? Path.Combine("Images", uniqueFileName) : null,
                    Availability = model.Availability,
                    TimeToPrepareMinutes = model.TimeToPrepareMinutes
                };
                _unitOFWork.DrinkRepository.AddDrink(newDrink);
                await _unitOFWork.Save();
                TempData["AddedSuccessfully"] = true;

            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Edit");
            }
            var drink = await _unitOFWork.DrinkRepository.GetDrinkById(id.Value);
            if (drink == null)
            {
                return NotFound();
            }

            var model = new DrinkViewModel
            {
                Id = drink.Id,
                Name = drink.Name,
                Description = drink.Description,
                Price = drink.Price,
                PictureUrl = drink.PictureUrl,
                Availability = drink.Availability,
                TimeToPrepareMinutes = drink.TimeToPrepareMinutes,

            };

            return View("Edit", model);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var drink = await _unitOFWork.DrinkRepository.GetDrinkById(id);



            var fileName = drink.PictureUrl;

            _unitOFWork.DrinkRepository.DeleteDrink(id);
            await _unitOFWork.Save();
            TempData["DeletedSuccessfully"] = true;

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);

            }



            return RedirectToAction("Index");
        }

    }
}
