using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblUnit
{
    public string UnitId { get; set; } = null!;

    public string UnitCd { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public virtual ICollection<TblPart> TblParts { get; set; } = new List<TblPart>();
}
