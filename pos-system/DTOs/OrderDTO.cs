using pos_system.Helpers;
using pos_system.Models;

namespace pos_system.DTOs
{
    public class OrderDTO
    {
        public string OrderId { get; set; } = Unique.ID();
        public string? UserID { get; set; }

        public DateTime OrderDate { get; set; } = new DateTime();
        public string OrderNumber { get; set; } = String.Empty;

        public string Status { get; set; } = String.Empty;

        public decimal TotalPrice { get; set; } = 0;
        public string Cashier { get; set; } = String.Empty;
        public string Type { get; set; } = "Cashier";
        public DateTime? ScheduledAt { get; set; }
        public string? Notes { get; set; } = String.Empty;
        public string? PreOrderStatus { get; set; } = String.Empty;
        public string? Comment { get; set; }

        public virtual ICollection<OrderItemDTO> TblOrderItems { get; set; } = [];
    }
}
