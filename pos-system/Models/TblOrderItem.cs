using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblOrderItem
{
    public string OrderItemId { get; set; } = Unique.ID();

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public string? VariantId { get; set; }

    public int Quantity { get; set; } = 0;

    public decimal UnitPrice { get; set; } = 0;

    public decimal SubTotal { get; set; } = 0;
    public string? Notes { get; set; }

    public virtual TblOrder Order { get; set; } = null!;

    public virtual ICollection<TblOrderItemAddon> TblOrderItemAddons { get; set; } = [];
}
