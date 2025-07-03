using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class OrderService(IOrderRepo orderRepo, IProductVariantRepo productVariantRepo, IProductRepo productRepo, IOrderNumberTrackerRepo orderNumberTrackerRepo) : IOrderService
    {
        readonly IOrderRepo _orderRepo = orderRepo;
        readonly IProductVariantRepo _productVariantRepo = productVariantRepo;
        readonly IProductRepo _productRepo = productRepo;
        readonly IOrderNumberTrackerRepo _orderNumberTrackerRepo = orderNumberTrackerRepo;
        private string _username = "System";

        public void SetUsername(string username)
        {
            _username = username;
            _orderRepo.GetRepo().SetUsername(username);
            _productVariantRepo.GetRepo().SetUsername(username);
            _productRepo.GetRepo().SetUsername(username);
        }

        public async Task CreateOrder(TblOrder order)
        {
            try
            {
                order.OrderNumber = await _orderNumberTrackerRepo.GetOrderNumber();
                order.OrderDate = DateTime.Now;
                order.Status = "Success";
                order.TotalPrice = order.TblOrderItems.Sum(oi => oi.SubTotal);
                order.Cashier = _username;
                order.TblOrderItems = order.TblOrderItems.Select(oi =>
                {
                    oi.OrderId = order.OrderId;
                    oi.Notes = oi.Notes ?? "";
                    return oi;
                }).ToList();

                await _orderRepo.GetRepo().Add(order);
                await ReduceProductStock(order).ConfigureAwait(false);
                await _orderNumberTrackerRepo.GenerateOrderNumber();
            }
            catch(Exception ex)
            {
                throw new Exception("Error creating order: " + ex.Message, ex);
            }
        }

        public async Task<List<TblOrder>> GetOrderHistory(string? fromDate, string? toDate)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;

                fromDate = new DateTime(year, month, DateTime.Now.Day).AddDays(-6).ToString("yyyy-MM-dd");
                toDate = new DateTime(year, month, DateTime.Now.Day).ToString("yyyy-MM-dd");
            }

            var history = await _orderRepo.GetOrdersByDate(fromDate, toDate).ConfigureAwait(false);
            history = history.Select(o => { o.TotalPrice = decimal.Round(o.TotalPrice, 0); return o; }).ToList();
            return history.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderNumber).ToList();
        }

        private async Task ReduceProductStock(TblOrder order)
        {
            foreach (var item in order.TblOrderItems)
            {
                if (string.IsNullOrEmpty(item.VariantId))
                {
                    var product = await _productRepo.GetRepo().GetById(item.ProductId).ConfigureAwait(false);
                    product.ProductStock -= item.Quantity;
                    await _productRepo.GetRepo().Update(product).ConfigureAwait(false);
                }
                else
                {
                    var productVariant = await _productVariantRepo.GetRepo().GetById(item.VariantId).ConfigureAwait(false);
                    productVariant.VariantStock -= item.Quantity;
                    await _productVariantRepo.GetRepo().Update(productVariant).ConfigureAwait(false);
                }
            }
        }

        public async Task<List<TblOrder>> GetPreOrder(string userId, string role)
        {
            return await _orderRepo.GetPreOrder(userId, role).ConfigureAwait(false);
        }

        public async Task UpdatePreOrderStatus(string orderId, string status, string comment)
        {
            await _orderRepo.UpdatePreOrderStatus(orderId, status, comment).ConfigureAwait(false);
        }
    }
}
