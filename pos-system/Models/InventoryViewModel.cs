using pos_system.DTOs;
using pos_system.Helpers;

namespace pos_system.Models
{
    public class InventoryViewModel
    {
        public List<PartDTO> PartList { get; set; }
        public List<TblPartType?> PartType { get; set; }
    }

    public class InventoryLogViewModel
    {
        public string NamaBarang { get; set; }
        public string Kategori { get; set; }
        public int StockIn { get; set; }
        public int StockOut { get; set; }
        public string Note { get; set; }
        public string InputedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
