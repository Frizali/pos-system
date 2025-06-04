using pos_system.Models;

namespace pos_system.Repository
{
    public interface ILogAuditRepo
    {
        ICrudRepo<TblLogAudit> GetRepo();
        Task<List<TblLogAudit>> GetLogs(string? action, string? entity, string? key, string? fromDate, string? toDate);
    }
}
