using DataLayer.Entities;

namespace BusinessLogicLayer.Repositories.Interfaces
{
    public interface IOfficeLocationRepository
    {
        Task<OfficeLocation> GetLocationById(int id);
        Task<IEnumerable<OfficeLocation>> GetAllLocations();
        void AddLocation(OfficeLocation location);
        void UpdateLocation(OfficeLocation location);
        void DeleteLocation(int id);
    }
}
