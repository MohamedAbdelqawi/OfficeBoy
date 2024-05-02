using BusinessLogicLayer.Repositories.Interfaces;
using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Repositories.implementationRepo
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly ApplicationDbContext _context;

        public DrinkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddDrink(Drink drink)
        {

            _context.Drinks.Add(drink);
        }

        public async void DeleteDrink(int id)
        {
            var drink = await _context.Drinks.FirstOrDefaultAsync(d => d.Id == id);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
            }
        }

        public async Task<IEnumerable<Drink>> GetAllDrinks(string? searchTerm)
        {
            return await _context.Drinks.Where(d => (string.IsNullOrEmpty(searchTerm) || d.Name.ToLower().Contains(searchTerm.ToLower())) || d.Price.ToString().ToLower().Contains(searchTerm.ToLower())).ToListAsync();
        }
        public async Task<IEnumerable<Drink>> GetAllAvailableDrinks(string? search)
        {
            return await _context.Drinks.Where(d => d.Availability == true && (string.IsNullOrEmpty(search) || d.Name.Contains(search, StringComparison.OrdinalIgnoreCase))).ToListAsync();
        }

        public async Task<Drink> GetDrinkById(int id)
        {
            return await _context.Drinks.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async void UpdateDrink(Drink drink)
        {
            var existingDrink = await _context.Drinks.FirstOrDefaultAsync(d => d.Id == drink.Id);
            if (existingDrink != null)
            {
                // Update properties
                existingDrink.Name = drink.Name;
                existingDrink.Description = drink.Description;
                existingDrink.Price = drink.Price;
                existingDrink.Availability = drink.Availability;
            }
        }
        public void save()
        {
            _context.SaveChanges();
        }









    }
}