using BusinessLogicLayer.Repositories.Interfaces;
using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Repositories.implementationRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.
                Include(o => o.Drink)
                .Where(o => o.DateTimeOfOrder.Date == DateTime.Today && o.Status != (OrderStatus.Delivered)).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersForUser(string userId)
        {
            return await _context.Orders.
                Include(o => o.Drink)
                .Where(o => o.DateTimeOfOrder.Date == DateTime.Today && o.EmployeeId == userId).ToListAsync();
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetPdfOrders(List<int> ids)
        {
            return await _context.Orders.Where(o => ids.Contains(o.Id)).ToListAsync();
        }

        public void UpdateOrderStatus(Order order)
        {
            _context.Orders.Update(order);

        }
    }
}