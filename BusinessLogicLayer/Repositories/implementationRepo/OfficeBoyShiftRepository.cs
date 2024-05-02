using BusinessLogicLayer.Repositories.Interfaces;
using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Repositories.implementationRepo
{
    public class OfficeBoyShiftRepository : IOfficeBoyShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public OfficeBoyShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OfficeBoyShift> GetShiftById(string id)
        {
            return await _context.OfficeBoyShifts.FirstOrDefaultAsync(x => x.EmployeeId == id);
        }
        public async Task<List<OfficeBoyShift>> GetShiftsByEmployeeIds(List<string> employeeIds)
        {
            return await _context.OfficeBoyShifts
                .Where(x => employeeIds.Contains(x.EmployeeId))
                .ToListAsync();
        }

        public async Task<IEnumerable<OfficeBoyShift>> GetAllShifts()
        {
            return await _context.OfficeBoyShifts.ToListAsync();
        }

        public void AddShift(OfficeBoyShift shift)
        {
            _context.OfficeBoyShifts.Add(shift);
        }

        public void UpdateShift(OfficeBoyShift shift)
        {
            _context.OfficeBoyShifts.Update(shift);
        }

        public void DeleteShift(int id)
        {
            var shift = _context.OfficeBoyShifts.Find(id);
            if (shift != null)
            {
                _context.OfficeBoyShifts.Remove(shift);
            }
        }
    }
}