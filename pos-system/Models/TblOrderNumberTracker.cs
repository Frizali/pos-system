using System;
using System.Collections.Generic;

namespace pos_system.Models;

public partial class TblOrderNumberTracker
{
    public DateTime DateID { get; set; }

    public int LastNumber { get; set; }
}
