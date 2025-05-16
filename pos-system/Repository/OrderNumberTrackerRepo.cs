
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class OrderNumberTrackerRepo(AppDbContext context) : IOrderNumberTrackerRepo
    {
        readonly AppDbContext _context = context;
        public async Task GenerateOrderNumber()
        {
            var today = DateTime.Today.Date;
            var tracker = await _context.TblOrderNumberTracker.FirstOrDefaultAsync(o => o.DateID == today);

            int newNumber;
            if (tracker == null)
            {
                newNumber = 1;
                tracker = new TblOrderNumberTracker
                {
                    DateID = today,
                    LastNumber = newNumber
                };
                _context.TblOrderNumberTracker.Add(tracker);
            }
            else
            {
                newNumber = tracker.LastNumber + 1;
                tracker.LastNumber = newNumber;
                _context.TblOrderNumberTracker.Update(tracker);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetOrderNumber()
        {
            var today = DateTime.Today.Date;
            var tracker = await _context.TblOrderNumberTracker.FirstOrDefaultAsync(o => o.DateID == today);

            int newNumber;
            if (tracker == null)
            {
                newNumber = 1;
                tracker = new TblOrderNumberTracker
                {
                    DateID = today,
                    LastNumber = newNumber
                };
                return $"{newNumber:D3}";
            }
            else
            {
                newNumber = tracker.LastNumber + 1;
                return $"{newNumber:D3}";
            }
        }
    }
}
