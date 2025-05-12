using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblProductCategory
{
    public string CategoryId { get; set; } = Unique.ID();

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; set; } = [];
}
