using pos_system.Helpers;
using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblPart
{
    public string PartId { get; set; } = Unique.ID();

    public string PartTypeId { get; set; } = null!;

    public string UnitId { get; set; } = null!;

    public string PartCd { get; set; } = null!;

    public string PartName { get; set; } = null!;

    public double PartQty { get; set; }

    public double LowerLimit { get; set; }

    public double Price { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual TblUnit Unit { get; set; } = null!;
}
