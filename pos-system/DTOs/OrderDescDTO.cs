namespace pos_system.DTOs
{
    public class OrderDescDTO
    {
        public string? OrderId { get; set; }
        public string  Username { get; set; }
        public string Email { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDecsDTO> items { get; set; } = new List<OrderItemDecsDTO>();
    }

    public class OrderItemDecsDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
