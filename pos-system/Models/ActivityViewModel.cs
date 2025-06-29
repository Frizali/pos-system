namespace pos_system.Models
{
    public class ActivityViewModel
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public List<TblOrder>? TblOrders { get; set; } = new List<TblOrder>();
        public List<TblLogAudit>? TblLogAudits { get; set; } = new List<TblLogAudit>();
        public List<InventoryViewModel>? TblInventoryPartMovements { get; set; } = new List<InventoryViewModel>();
    }
}
