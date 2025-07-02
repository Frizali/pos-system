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
        public TotalSalesAmount TotalSalesAmount { get; set; } = new();
        public TotalProductSales TotalProductSales { get; set; } = new();
        public TotalCustomers TotalCustomers { get; set; } = new();
    }

    public class TotalSalesAmount
    {
        public decimal Amount { get; set; } = 0;
        public bool IsGrowth { get; set; } = false;
        public decimal GrowthAmount { get; set; } = 0;
        public decimal GrowthPercentage { get; set; } = 0;
    }

    public class TotalProductSales
    {
        public decimal Amount { get; set; } = 0;
        public bool IsGrowth { get; set; } = false;
        public decimal GrowthAmount { get; set; } = 0;
        public decimal GrowthPercentage { get; set; } = 0;
    }

    public class TotalCustomers
    {
        public decimal Amount { get; set; } = 0;
        public bool IsGrowth { get; set; } = false;
        public decimal GrowthAmount { get; set; } = 0;
        public decimal GrowthPercentage { get; set; } = 0;
    }

    public class ChartViewModel
    {
        public List<string> Labels { get; set; } = [];
        public List<int> Data { get; set; } = [];
        public List<string> PointStyles { get; set; } = [];
    }

    public class FavoriteProduct
    {
        public string? ProductId { get; set; }
        public string? VariantId { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public string CategoryName { get; set; } = String.Empty;
        public int TotalOrder { get; set; } = 0;
    }
}