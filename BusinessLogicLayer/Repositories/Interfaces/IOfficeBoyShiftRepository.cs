using DataLayer.Entities;

namespace BusinessLogicLayer.Repositories.Interfaces
{
    public interface IOfficeBoyShiftRepository
    {
        Task<OfficeBoyShift> GetShiftById(string id);
        Task<IEnumerable<OfficeBoyShift>> GetAllShifts();
        Task<List<OfficeBoyShift>> GetShiftsByEmployeeIds(List<string> employeeIds);
        void AddShift(OfficeBoyShift shift);
        void UpdateShift(OfficeBoyShift shift);
        void DeleteShift(int id);
    }
}
