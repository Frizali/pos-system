using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class LogAuditRepo(AppDbContext context ,ICrudRepo<TblLogAudit> repo) : ILogAuditRepo
    {
        private readonly AppDbContext _context = context;
        private readonly ICrudRepo<TblLogAudit> _repo = repo;

        public ICrudRepo<TblLogAudit> GetRepo()
        {
            return _repo;
        }

        public async Task<List<TblLogAudit>> GetLogs(string? action, string? entity, string? key, string? fromDate, string? toDate)
        {
            return await _context.TblLogAudit
                .Where(log => (log.LogAction != "Modified" || log.LogFldOldValue != log.LogFldNewValue) && (string.IsNullOrEmpty(action) || log.LogAction == action) &&
                              (string.IsNullOrEmpty(entity) || log.LogEntityName == entity) &&
                              (string.IsNullOrEmpty(key) || log.LogKeyName == key) &&
                              (string.IsNullOrEmpty(fromDate) || log.LogDateTime >= DateTime.Parse(fromDate)) &&
                              (string.IsNullOrEmpty(toDate) || log.LogDateTime <= DateTime.Parse(toDate)))
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
