using pos_system.Models;

namespace pos_system.Repository
{
    public interface IOrderRepo
    {
        ICrudRepo<TblOrder> GetRepo();
        Task<decimal> GetTotalSalesAmount(string fromDate, string toDate);
        Task<int> GetTotalProductSales(string fromDate, string toDate);
        Task<int> GetTotalCustomers(string fromDate, string toDate);
    }
}
