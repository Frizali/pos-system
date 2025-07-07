namespace pos_system.Models
{
    public class PreOrderViewModel
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string Status { get; set; } = "All";
        public List<TblOrder> Orders { get; set; } = new();
    }
}
