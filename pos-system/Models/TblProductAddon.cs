namespace pos_system.Models;

public partial class TblProductAddon
{
    public string? AddonId { get; set; } = null!;

    public string? ProductId { get; set; } = null!;

    public string AddonName { get; set; } = null!;

    public double AddonPrice { get; set; }

    public int AddonStock { get; set; }

    public bool IsLimitedStock { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public virtual TblProduct? Product { get; set; } = null!;
}
