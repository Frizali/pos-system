using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class DashboardService(IOrderRepo orderRepo) : IDashboardService
    {
        readonly IOrderRepo _orderRepo = orderRepo;

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

            return new DashboardViewModel()
            {
                SalesAnalytics = new SalesAnalytics()
                {
                    TotalSalesAmount = decimal.Round(totalSalesAmount, 0),
                    TotalProductSales = totalProductSales,
                    TotalCustomers = totalCustomers
                },
                Chart = GetChartViewModel(fromDate, toDate, orders)
            };
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
