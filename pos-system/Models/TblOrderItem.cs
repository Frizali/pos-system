using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblOrderItem
{
    public string OrderItemId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public string? VariantId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }

    public virtual TblOrder Order { get; set; } = null!;

    public virtual ICollection<TblOrderItemAddon> TblOrderItemAddons { get; set; } = new List<TblOrderItemAddon>();
}
