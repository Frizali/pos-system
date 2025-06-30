namespace pos_system.Models
{
    public class SnapTokenModel
    {
        public string orderId { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime? scheduledAt { get; set; }
        public string? notes { get; set; }
        public string? type { get; set; }
        public List<TblOrderItem> TblOrderItems { get; set; } = new();
    }
}
