using pos_system.DTOs;
using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class DashboardService(IOrderRepo orderRepo, IProductRepo productRepo) : IDashboardService
    {
        readonly IOrderRepo _orderRepo = orderRepo;
        readonly IProductRepo _productRepo = productRepo;

        public async Task<DashboardViewModel> DashboardViewModel(string? fromDate, string? toDate)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;

                fromDate = new DateTime(year, month, DateTime.Now.Day).AddDays(-6).ToString("yyyy-MM-dd");
                toDate = new DateTime(year, month, DateTime.Now.Day).ToString("yyyy-MM-dd");
            }

            var totalSalesAmount = await _orderRepo.GetTotalSalesAmount(fromDate, toDate);
            var totalProductSales = await _orderRepo.GetTotalProductSales(fromDate, toDate);
            var totalCustomers = await _orderRepo.GetTotalCustomers(fromDate, toDate);
            var orders = await _orderRepo.GetOrdersByDate(fromDate, toDate);
            var productDTOs = await _productRepo.ProductDetailsDTO();

            return new DashboardViewModel()
            {
                FromDate = fromDate,
                ToDate = toDate,
                SalesAnalytics = new SalesAnalytics()
                {
                    TotalSalesAmount = decimal.Round(totalSalesAmount, 0),
                    TotalProductSales = totalProductSales,
                    TotalCustomers = totalCustomers
                },
                Chart = GetChartViewModel(fromDate, toDate, orders),
                FavoriteProducts = GetFavoriteProduct(orders, productDTOs)
            };
        }

        private static List<FavoriteProduct> GetFavoriteProduct(List<TblOrder> orders, List<ProductDTO> productDTOs)
        {
            var orderProductID = orders.SelectMany(o => o.TblOrderItems).Where(oi => oi.VariantId is null).Select(oi => new { ProductID = oi.ProductId, oi.Quantity }).ToList();
            var orderVariantID = orders.SelectMany(o => o.TblOrderItems).Where(oi => oi.VariantId is not null).Select(oi => new { ProductID = oi.ProductId, VariantID = oi.VariantId, oi.Quantity }).ToList();

            var resProduct = from op in orderProductID
                             join p in productDTOs on op.ProductID equals p.ProductId
                             select new
                             {
                                 op,
                                 p
                             } into joined
                             group joined by joined.op.ProductID into g
                             select new FavoriteProduct()
                             {
                                 ProductName = g.First().p.ProductName,
                                 CategoryName = g.First().p.Category.CategoryName,
                                 TotalOrder = g.Sum(x => x.op.Quantity),
                             };

            var resVariant = from ov in orderVariantID
                             join v in productDTOs.SelectMany(p => p.ProductVariants).ToList() on ov.VariantID equals v.VariantId
                             join p in productDTOs on v.ProductId equals p.ProductId
                             select new { p,v, ov } into vpJoined
                             group vpJoined by vpJoined.v.VariantId into g
                             select new FavoriteProduct()
                             {
                                 ProductName = g.First().p.ProductName + $" ({g.First().v.Sku})",
                                 CategoryName = g.First().p.Category.CategoryName,
                                 TotalOrder = g.Sum(x => x.ov.Quantity),
                             };

            return resProduct.Concat(resVariant).Take(5).OrderByDescending(fp => fp.TotalOrder).ToList();
        }

        private ChartViewModel GetChartViewModel(string fromDate, string toDate, List<TblOrder> orders)
        {
            var chart = new ChartViewModel();
            var labels = new List<string>();
            var data = new List<int>();
            var pointStyles = new List<string>();

            var datediff = (Convert.ToDateTime(toDate).Date - Convert.ToDateTime(fromDate).Date).Days;

            for(var i = 0; i <= datediff; i++)
            {
                var label = Convert.ToDateTime(fromDate).AddDays(i).ToString("MMMM dd");
                var amount = orders.Where(o => DateOnly.FromDateTime(o.OrderDate) == DateOnly.Parse(Convert.ToDateTime(fromDate).AddDays(i).ToString("yyyy-MM-dd"))).Sum(o => o.TotalPrice);
                labels.Add(label);
                data.Add((int)amount);
            }

            chart.Labels = labels;
            chart.Data = data;
            chart.PointStyles = pointStyles;

            return chart;
        }
    }
}
