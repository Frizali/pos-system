using pos_system.Models;

namespace pos_system.Services
{
    public interface ILogAuditService
    {
        Task<List<TblLogAudit>> GetLogs(string? action, string? entity, string? key, string? fromDate, string? toDate);
    }
}
