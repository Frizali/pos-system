using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblOrderItemAddon
{
    public string ItemAddonId { get; set; } = null!;

    public string OrderItemId { get; set; } = null!;

    public string AddonId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal AddonPrice { get; set; }

    public decimal SubTotal { get; set; }

    public virtual TblOrderItem OrderItem { get; set; } = null!;
}
