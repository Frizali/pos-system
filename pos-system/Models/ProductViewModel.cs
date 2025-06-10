using pos_system.DTOs;

namespace pos_system.Models
{
    public class ProductListViewModel
    {
        public List<ProductDTO> Products { get; set; }
        public List<ProductCategoryDTO> ProductCategories { get; set; }
        public string OrderNumber { get; set; } = String.Empty;
        public string? ProductFilter { get; set; } = String.Empty;
    }
}
