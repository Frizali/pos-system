using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class DashboardService(IOrderRepo orderRepo) : IDashboardService
    {
        readonly IOrderRepo _orderRepo = orderRepo;

        public async Task<DashboardViewModel> DashboardViewModel(string fromDate, string toDate)
        {
            var totalSalesAmount = await _orderRepo.GetTotalSalesAmount(fromDate, toDate);
            var totalProductSales = await _orderRepo.GetTotalProductSales(fromDate, toDate);
            var totalCustomers = await _orderRepo.GetTotalCustomers(fromDate, toDate);

            return new DashboardViewModel()
            {
                SalesAnalytics = new SalesAnalytics()
                {
                    TotalSalesAmount = decimal.Round(totalSalesAmount,0),
                    TotalProductSales = totalProductSales,
                    TotalCustomers = totalCustomers
                }
            };
        }
    }
}
