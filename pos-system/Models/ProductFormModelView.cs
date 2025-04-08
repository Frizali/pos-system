namespace pos_system.Models
{
    public class ProductFormModelView
    {
        public TblProduct Product { get; set; }
        public List<TblVariantGroup>? VariantGroups { get; set; } = new List<TblVariantGroup>();
        public List<TblVariantOption>? VariantOptions { get; set; } = new List<TblVariantOption>();
        public List<TblProductCategory>? ProductCategories { get; set; } = new List<TblProductCategory>();
        public List<TblProductVariant>? ProductVariants { get; set; } = new List<TblProductVariant>();
        public List<TblProductAddon>? ProductAddons { get; set; } = new List<TblProductAddon>();
    }
}
