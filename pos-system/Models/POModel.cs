public class POModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime TanggalPO { get; set; }
    public List<PODetail> Detail { get; set; }
    public string Note { get; set; }
    public int Total { get; set; }
    public string Status { get; set; }
}

public class PODetail
{
    public string Produk { get; set; }
    public string Variant { get; set; }
    public string Note { get; set; }
    public int Qty { get; set; }
}