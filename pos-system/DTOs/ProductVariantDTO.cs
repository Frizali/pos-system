using pos_system.Helpers;

namespace pos_system.DTOs
{
    public class ProductVariantDTO
    {
        public string? VariantId { get; set; } = Unique.ID();

        public string? ProductId { get; set; } = null!;

        public string? Sku { get; set; } = null!;

        public double? VariantPrice { get; set; } = 0;

        public int? VariantStock { get; set; } = 0;

        public bool IsLimitedStock { get; set; } = false;

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
