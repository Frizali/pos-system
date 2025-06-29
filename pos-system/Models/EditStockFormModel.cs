using pos_system.Helpers;

namespace pos_system.Models
{
    public class EditStockFormModal
    {
        public string PartMovementId { get; set; } = Unique.ID();
        public string PartId { get; set; } = null!;
        public string? PartName { get; set; } = null;
        public string? PartTypeName { get; set; } = null;
        public string? UnitCd { get; set; } = null;
        public double? PartQty { get; set; } = null;
        public double? Price { get; set; } = null;
        public double? LowerLimit { get; set; } = null;
        public int PartMovType { get; set; }
        public int PartMovQty { get; set; }
        public string? Remark { get; set; }
        public string? InputedBy { get; set; }
    }

    public enum PartMovTypeEnum
    {
        In = 1,
        Out
    }
}
