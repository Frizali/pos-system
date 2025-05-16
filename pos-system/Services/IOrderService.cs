using pos_system.Models;

namespace pos_system.Services
{
    public interface IOrderService
    {
        Task CreateOrder(TblOrder order);
    }
}
