using System.ComponentModel.DataAnnotations;

namespace pos_system.Models;

public partial class TblProduct
{
    public string? ProductId { get; set; } = null!;

    public string CategoryId { get; set; } = null!;

    public string? ProductCode { get; set; } = null!;

    [Required(ErrorMessage = "Product name is required")]
    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public int? ProductStock { get; set; }

    public bool IsLimitedStock { get; set; }

    public bool IsAvailable { get; set; }

    public double? Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual TblProductCategory? Category { get; set; } = null!;

    public virtual ICollection<TblProductVariant> TblProductVariants { get; set; } = [];

    public virtual ICollection<TblVariantGroup> TblVariantGroups { get; set; } = [];

    public virtual ICollection<TblProductAddon> TblProductAddons { get; set; } = [];
}
