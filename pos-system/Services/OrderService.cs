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

        public async Task CreateOrder(TblOrder order)
        {
            try
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
                await ReduceProductStock(order).ConfigureAwait(false);
                await _orderNumberTrackerRepo.GenerateOrderNumber();
            }
            catch(Exception ex)
            {
                throw new Exception("Error creating order: " + ex.Message, ex);
            }
        }

        private async Task ReduceProductStock(TblOrder order)
        {
            foreach (var item in order.TblOrderItems)
            {
                if (item.VariantId is null)
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
    }
}
