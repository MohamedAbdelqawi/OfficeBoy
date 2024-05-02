using BusinessLogicLayer.Repositories.Interfaces;

namespace BusinessLogicLayer
{
    public interface IUnitOFWork : IDisposable
    {
        public IOrderRepository OrderRepository { get; }
        public IOfficeLocationRepository OfficeLocationRepository { get; }
        public IOfficeBoyShiftRepository OfficeBoyShiftRepository { get; }
        public IDrinkRepository DrinkRepository { get; }

        Task<int> Save();
    }
}
