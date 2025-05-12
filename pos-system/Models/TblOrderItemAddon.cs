using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblOrderItemAddon
{
    public string ItemAddonId { get; set; } = Unique.ID();

    public string OrderItemId { get; set; } = null!;

    public string AddonId { get; set; } = null!;

    public int Quantity { get; set; } = 0;

    public decimal AddonPrice { get; set; } = 0;

    public decimal SubTotal { get; set; } = 0;

    public virtual TblOrderItem OrderItem { get; set; } = new TblOrderItem();
}
