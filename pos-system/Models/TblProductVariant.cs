using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblProductVariant
{
    public string? VariantId { get; set; } = Unique.ID();

    public string? ProductId { get; set; } = null!;

    public string Sku { get; set; } = "N/A";

    public double? VariantPrice { get; set; } = 0;

    public int? VariantStock { get; set; } = 0;

    public bool IsLimitedStock { get; set; } = false;

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } 

    public DateTime UpdatedAt { get; set; }

    public virtual TblProduct? Product { get; set; } = null!;
}
