using DataLayer.Entities;

namespace BusinessLogicLayer.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetPdfOrders(List<int> ids);
        Task<Order> GetOrderById(int orderId);
        void UpdateOrderStatus(Order order);
        void AddOrder(Order order);
        Task<IEnumerable<Order>> GetAllOrdersForUser(string userId);
    }
}
