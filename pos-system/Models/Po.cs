namespace pos_system.Models
{
    public class Po
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public bool IsFavorite { get; set; }
    }
}
