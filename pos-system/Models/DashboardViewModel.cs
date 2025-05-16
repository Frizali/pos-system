namespace pos_system.Models
{
    public class DashboardViewModel
    {
        public SalesAnalytics SalesAnalytics { get; set; } = new();
    }

    public class SalesAnalytics
    {
        public decimal TotalSalesAmount { get; set; } = 0;  
        public int TotalProductSales { get; set; } = 0;
        public int TotalCustomers { get; set; } = 0;
    }
}