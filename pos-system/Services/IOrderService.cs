using pos_system.Models;

namespace pos_system.Services
{
    public interface IOrderService
    {
        Task CreateOrder(TblOrder order);
        Task<List<TblOrder>> GetOrderHistory(string? fromDate, string? toDate);
    }
}
