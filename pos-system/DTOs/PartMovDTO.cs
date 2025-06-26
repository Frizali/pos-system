namespace pos_system.DTOs
{
    public class PartMovDTO
    {
        public string PartName { get; set; }
        public string Category { get; set; }
        public string UnitCD { get; set; }
        public double LastPartQry { get; set; }
        public int StockIn { get; set; }
        public int StockOut { get; set; }
        public double CurrPartQry { get; set; }
        public string Note { get; set; }
        public string InputedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
