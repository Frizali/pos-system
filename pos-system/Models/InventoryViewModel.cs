namespace pos_system.Models
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string NamaBarang { get; set; }
        public string Kategori { get; set; }
        public string Unit { get; set; }
        public decimal Harga { get; set; }
        public string Note { get; set; }
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
