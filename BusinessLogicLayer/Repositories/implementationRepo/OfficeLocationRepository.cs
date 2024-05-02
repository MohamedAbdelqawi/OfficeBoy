using BusinessLogicLayer.Repositories.Interfaces;
using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Repositories.implementationRepo
{

    public class OfficeLocationRepository : IOfficeLocationRepository
    {

        private readonly ApplicationDbContext _context;

        public OfficeLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OfficeLocation> GetLocationById(int id)
        {
            return await _context.OfficeLocations.FindAsync(id);
        }

        public async Task<IEnumerable<OfficeLocation>> GetAllLocations()
        {
            return await _context.OfficeLocations.ToListAsync();
        }

        public void AddLocation(OfficeLocation location)
        {
            _context.OfficeLocations.Add(location);
         
        }

        public void UpdateLocation(OfficeLocation location)
        {
            _context.OfficeLocations.Update(location);
            
        }

        public void DeleteLocation(int id)
        {
            var location = _context.OfficeLocations.Find(id);
            if (location != null)
            {
                _context.OfficeLocations.Remove(location);
              
            }
        }
    }
}
