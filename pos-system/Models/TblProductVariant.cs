using System.Text.Json.Serialization;

namespace pos_system.Models;

public partial class TblProductVariant
{
    public string? VariantId { get; set; } = null!;

    public string? ProductId { get; set; } = null!;

    public string? Sku { get; set; } = null!;

    public double? VariantPrice { get; set; }

    public int? VariantStock { get; set; }

    public bool IsLimitedStock { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual TblProduct? Product { get; set; } = null!;
}
