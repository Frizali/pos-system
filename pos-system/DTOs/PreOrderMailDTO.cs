namespace pos_system.DTOs
{
    public class PreOrderMailDTO
    {
        public string  Username { get; set; }
        public string Email { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public decimal TotalPrice { get; set; }
        public List<PreOrderItemDTO> items { get; set; } = new List<PreOrderItemDTO>();
    }

    public class PreOrderItemDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
