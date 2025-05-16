using pos_system.Helpers;

namespace pos_system.DTOs
{
    public class ProductCategoryDTO
    {
        public string CategoryId { get; set; } = Unique.ID();

        public string CategoryName { get; set; } = String.Empty;

        public string? CategoryDescription { get; set; }

        public int TotalProducts { get; set; } = 0;
    }
}
