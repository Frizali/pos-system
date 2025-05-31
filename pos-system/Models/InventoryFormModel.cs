namespace pos_system.Models
{
    public class InventoryFormModel
    {
        public TblPart? Part { get; set; }
        public List<TblPartType?> PartTypes { get; set; }
        public List<TblUnit?> Units { get; set; }
    }
}
