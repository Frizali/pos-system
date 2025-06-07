using pos_system.DTOs;

namespace pos_system.Models
{
    public class InventoryMoveViewModel
    {
        public List<PartMovDTO> PartMovs { get; set; }
        public List<TblPart> Parts { get; set; }
        public List<TblPartType> PartTypes { get; set; }
    }
}
