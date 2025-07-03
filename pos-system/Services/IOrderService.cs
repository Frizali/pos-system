using pos_system.Models;

namespace pos_system.Services
{
    public interface IOrderService
    {
        void SetUsername(string username);
        Task CreateOrder(TblOrder order);
        Task<List<TblOrder>> GetOrderHistory(string? fromDate, string? toDate);
        Task<List<TblOrder>> GetPreOrder(string userId, string role);
        Task UpdatePreOrderStatus(string orderId, string status, string comment);
    }
}
