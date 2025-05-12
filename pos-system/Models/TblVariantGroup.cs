using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblVariantGroup
{
    public string? GroupId { get; set; } = Unique.ID();

    public string? ProductId { get; set; } = null!;

    public string VariantName { get; set; } = null!;

    public virtual TblProduct? Product { get; set; } = null!;

    public virtual ICollection<TblVariantOption>? TblVariantOptions { get; set; } = [];
}
