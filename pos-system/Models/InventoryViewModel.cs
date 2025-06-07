using pos_system.DTOs;
using pos_system.Helpers;

namespace pos_system.Models
{
    public class InventoryViewModel
    {
        public List<PartDTO> PartList { get; set; }
        public List<TblPartType?> PartType { get; set; }
    }
}
