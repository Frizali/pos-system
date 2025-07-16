using pos_system.Helpers;

namespace pos_system.DTOs
{
    public class OrderItemDTO
    {
        public string OrderItemId { get; set; } = Unique.ID();

        public string OrderId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public string? VariantId { get; set; }

        public int Quantity { get; set; } = 0;

        public decimal UnitPrice { get; set; } = 0;

        public decimal SubTotal { get; set; } = 0;
        public string? Notes { get; set; }
    }
}
