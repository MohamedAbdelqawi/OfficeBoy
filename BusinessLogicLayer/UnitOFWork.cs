using BusinessLogicLayer.Repositories.implementationRepo;
using BusinessLogicLayer.Repositories.Interfaces;
using DataLayer.Context;

namespace BusinessLogicLayer
{
    public class UnitOFWork : IUnitOFWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IOrderRepository OrderRepository { get; private set; }
        public IOfficeLocationRepository OfficeLocationRepository { get; private set; }
        public IOfficeBoyShiftRepository OfficeBoyShiftRepository { get; private set; }
        public IDrinkRepository DrinkRepository { get; private set; }



        public UnitOFWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            OrderRepository = new OrderRepository(applicationDbContext);
            OfficeLocationRepository = new OfficeLocationRepository(applicationDbContext);
            OfficeBoyShiftRepository = new OfficeBoyShiftRepository(applicationDbContext);
            DrinkRepository = new DrinkRepository(applicationDbContext);

        }

        public async Task<int> Save()
        {
            return await _applicationDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }
    }
}
