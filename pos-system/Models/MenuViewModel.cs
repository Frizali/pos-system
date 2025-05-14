using pos_system.DTOs;

namespace pos_system.Models
{
    public class MenuViewModel
    {
        public List<ProductDTO> Products { get; set; }
        public List<ProductCategoryDTO> ProductCategories { get; set; }
    }
}
