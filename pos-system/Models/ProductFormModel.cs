namespace pos_system.Models
{
    public class ProductFormModel
    {
        public TblProduct Product { get; set; }
        public List<TblVariantGroup>? VariantGroups { get; set; } = [];
        public List<TblVariantOption>? VariantOptions { get; set; } = [];
        public List<TblProductCategory>? ProductCategories { get; set; } = [];
        public List<TblProductVariant>? ProductVariants { get; set; } = [];
        public List<TblProductAddon>? ProductAddons { get; set; } = [];
    }
}
