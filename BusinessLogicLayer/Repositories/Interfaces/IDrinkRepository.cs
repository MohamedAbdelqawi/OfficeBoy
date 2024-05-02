using DataLayer.Entities;

namespace BusinessLogicLayer.Repositories.Interfaces
{
    public interface IDrinkRepository
    {
        Task<Drink> GetDrinkById(int id);
        Task<IEnumerable<Drink>> GetAllDrinks(string? searchTerm);
        Task<IEnumerable<Drink>> GetAllAvailableDrinks(string? search);
        void AddDrink(Drink drink);
        void UpdateDrink(Drink drink);
        void DeleteDrink(int id);
        void save();




    }
}
