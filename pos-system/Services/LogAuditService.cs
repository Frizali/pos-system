using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class LogAuditService(ILogAuditRepo repo) : ILogAuditService
    {
        private readonly ILogAuditRepo _repo = repo;
        public async Task<List<TblLogAudit>> GetLogs(string? action, string? entity, string? key, string? fromDate, string? toDate)
        {
            return await _repo.GetLogs(action, entity, key, fromDate, toDate).ConfigureAwait(false);
        }
    }
}
