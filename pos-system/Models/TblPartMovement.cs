using pos_system.Helpers;
using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblPartMovement
{
    public string PartMovementId { get; set; } = Unique.ID();

    public string PartId { get; set; } = null!;

    public int PartMovType { get; set; }

    public int PartMovQty { get; set; }

    public string? Remark { get; set; }

    public string? InputedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
