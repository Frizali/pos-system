using pos_system.Helpers;

namespace pos_system.DTOs
{
    public class PartListDTO
    {
        public string PartId { get; set; } = Unique.ID();

        public string PartTypeId { get; set; } = null!;
        public string PartTypeName { get; set; } = null!;

        public string UnitId { get; set; } = null!;
        public string UnitName { get; set; } = null!;

        public string PartCd { get; set; } = null!;

        public string PartName { get; set; } = null!;

        public double PartQty { get; set; }

        public double Price { get; set; }

        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
