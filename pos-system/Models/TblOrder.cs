using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblOrder
{
    public string OrderId { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public virtual ICollection<TblOrderItem> TblOrderItems { get; set; } = new List<TblOrderItem>();
}
