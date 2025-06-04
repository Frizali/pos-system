using pos_system.Helpers;
using System.ComponentModel.DataAnnotations;

namespace pos_system.Models
{
    public class TblLogAudit
    {
        [Key]
        public Guid LogID { get; set; } = Guid.NewGuid();
        public string LogAction { get; set; } = null!;
        public string LogEntityName { get; set; } = null!;
        public string LogKeyName { get; set; } = null!;
        public string LogKeyValue { get; set; } = null!;
        public string LogUsername { get; set; } = null!;
        public string? LogFldName { get; set; } = String.Empty;
        public string? LogFldOldValue { get; set; } = String.Empty;
        public string? LogFldNewValue { get; set; } = String.Empty;
        public DateTime LogDateTime { get; set; } = DateTime.Now;
    }
}
