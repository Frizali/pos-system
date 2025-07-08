using System.ComponentModel.DataAnnotations;

namespace pos_system.Models
{
    public class InventoryFormModel
    {
        [Required(ErrorMessage = "wajib diisi.")]
        public TblPart? Part { get; set; }

        [Required(ErrorMessage = "wajib diisi.")]
        public List<TblPartType?> PartTypes { get; set; }

        [Required(ErrorMessage = "wajib diisi.")]
        public List<TblUnit?> Units { get; set; }
    }
}
