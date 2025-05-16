using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblOrder
{
    public string OrderId { get; set; } = Unique.ID();

    public DateTime OrderDate { get; set; } = new DateTime();
    public string OrderNumber { get; set; } = String.Empty;

    public string Status { get; set; } = String.Empty;

    public decimal TotalPrice { get; set; } = 0;

    public virtual ICollection<TblOrderItem> TblOrderItems { get; set; } = [];
}
