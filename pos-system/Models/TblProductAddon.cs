using pos_system.Helpers;

namespace pos_system.Models;

public partial class TblProductAddon
{
    public string? AddonId { get; set; } = Unique.ID();

    public string? ProductId { get; set; } = null!;

    public string AddonName { get; set; } = null!;

    public double AddonPrice { get; set; } = 0;

    public int AddonStock { get; set; } = 0;

    public bool IsLimitedStock { get; set; } = false;

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public virtual TblProduct? Product { get; set; } = null!;
}
