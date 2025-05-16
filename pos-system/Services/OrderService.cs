using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class OrderService(IOrderRepo orderRepo, IOrderNumberTrackerRepo orderNumberTrackerRepo) : IOrderService
    {
        readonly IOrderRepo _orderRepo = orderRepo;
        readonly IOrderNumberTrackerRepo _orderNumberTrackerRepo = orderNumberTrackerRepo;

        public async Task CreateOrder(TblOrder order)
        {
            order.OrderNumber = await _orderNumberTrackerRepo.GetOrderNumber();
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";
            order.TotalPrice = order.TblOrderItems.Sum(oi => oi.SubTotal);
            order.TblOrderItems = order.TblOrderItems.Select(oi =>
            {
                oi.OrderId = order.OrderId;
                return oi;
            }).ToList();

            await _orderRepo.GetRepo().Add(order);
            await _orderNumberTrackerRepo.GenerateOrderNumber();
        }
    }
}
