using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblOrder
{
    public string OrderId { get; set; } = Unique.ID();
    public string? UserID { get; set; }

    public DateTime OrderDate { get; set; } = new DateTime();
    public string OrderNumber { get; set; } = String.Empty;

    public string Status { get; set; } = String.Empty;

    public decimal TotalPrice { get; set; } = 0;
    public string Cashier { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
    public DateTime? ScheduledAt { get; set; }
    public string? Notes { get; set; } = String.Empty;

    public virtual ICollection<TblOrderItem> TblOrderItems { get; set; } = [];
}
