namespace pos_system.Models
{
    public class DashboardViewModel
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public SalesAnalytics SalesAnalytics { get; set; } = new();
        public ChartViewModel Chart { get; set; } = new();
        public List<FavoriteProduct> FavoriteProducts { get; set; } = new();
    }

    public class SalesAnalytics
    {
        public decimal TotalSalesAmount { get; set; } = 0;  
        public int TotalProductSales { get; set; } = 0;
        public int TotalCustomers { get; set; } = 0;
    }

    public class ChartViewModel
    {
        public List<string> Labels { get; set; } = [];
        public List<int> Data { get; set; } = [];
        public List<string> PointStyles { get; set; } = [];
    }

    public class FavoriteProduct
    {
        public string ProductName { get; set; } = String.Empty;
        public string CategoryName { get; set; } = String.Empty;
        public int TotalOrder { get; set; } = 0;
    }
}