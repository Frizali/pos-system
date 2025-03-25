using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblVariantOption
{
    public string OptionId { get; set; } = null!;

    public string GroupId { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual TblVariantGroup Group { get; set; } = null!;
}
