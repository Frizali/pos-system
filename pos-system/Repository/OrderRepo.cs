using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class OrderRepo(AppDbContext context, ICrudRepo<TblOrder> repo) : IOrderRepo
    {
        readonly ICrudRepo<TblOrder> _repo = repo;
        readonly AppDbContext _context = context;

        public ICrudRepo<TblOrder> GetRepo()
        {
            return _repo;
        }

        public async Task<List<TblOrder>> GetOrdersByDate(string fromDate, string toDate)
        {
            return await _context.TblOrder.Where(o => DateOnly.FromDateTime(o.OrderDate) >= DateOnly.Parse(fromDate) && DateOnly.FromDateTime(o.OrderDate) <= DateOnly.Parse(toDate))
                .Include(o => o.TblOrderItems)
                .ToListAsync();
        }

        public async Task<int> GetTotalCustomers(string fromDate, string toDate)
        {
            return await _context.TblOrder.Where(o => DateOnly.FromDateTime(o.OrderDate) >= DateOnly.Parse(fromDate) && DateOnly.FromDateTime(o.OrderDate) <= DateOnly.Parse(toDate)).CountAsync();
        }

        public async Task<int> GetTotalProductSales(string fromDate, string toDate)
        {
            List<string> orderIDs = await _context.TblOrder.Where(o => DateOnly.FromDateTime(o.OrderDate) >= DateOnly.Parse(fromDate) && DateOnly.FromDateTime(o.OrderDate) <= DateOnly.Parse(toDate)).Select(o => o.OrderId).ToListAsync();
            return await _context.TblOrderItem.Where(oi => orderIDs.Contains(oi.OrderId)).SumAsync(oi => oi.Quantity);
        }

        public async Task<decimal> GetTotalSalesAmount(string fromDate, string toDate)
        {
            return await _context.TblOrder.Where(o => DateOnly.FromDateTime(o.OrderDate) >= DateOnly.Parse(fromDate) && DateOnly.FromDateTime(o.OrderDate) <= DateOnly.Parse(toDate)).SumAsync(o => o.TotalPrice);
        }

        public async Task<List<TblOrder>> GetPreOrder(string fromDate, string toDate, string status, string userId, string role)
        {
            return await _context.TblOrder.Where(o => o.Type == "PreOrder" && (role != "User" || o.UserID == userId) && (status == "All" || o.PreOrderStatus == status) && (DateOnly.FromDateTime(o.ScheduledAt ?? DateTime.Now) >= DateOnly.Parse(fromDate) && DateOnly.FromDateTime(o.ScheduledAt ?? DateTime.Now) <= DateOnly.Parse(toDate))).ToListAsync();
        }

        public async Task UpdatePreOrderStatus(string orderId, string status, string? comment)
        {
            var order = await _context.TblOrder.FindAsync(orderId);
            order.PreOrderStatus = status;
            if(!String.IsNullOrEmpty(comment))
                order.Comment = comment;

            await _context.SaveChangesAsync();
        }
    }
}
