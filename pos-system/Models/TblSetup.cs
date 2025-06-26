using pos_system.Helpers;
using System.ComponentModel.DataAnnotations;

namespace pos_system.Models
{
    public class TblSetup
    {
        [Key]
        public string ID { get; set; } = Unique.ID(); 
        public string CompanyName { get; set; } = String.Empty;
        public string CompanyAddress { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
    }
}
