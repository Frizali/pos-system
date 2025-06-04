using pos_system.Helpers;

namespace pos_system.Models
{
    public class TblLogAudit
    {
        public string LogID { get; set; } = Unique.ID();
        public string LogAction { get; set; } = null!;
        public string LogEntityName { get; set; } = null!;
        public string LogKeyName { get; set; } = null!;
        public string LogKeyValue { get; set; } = null!;
        public string LogUsername { get; set; } = null!;
        public string LogFldName { get; set; } = null!;
        public string LogFldOldValue { get; set; } = null!;
        public string LogFldNewValue { get; set; } = null!;
        public DateTime LogDateTime { get; set; } = DateTime.Now;
    }
}
