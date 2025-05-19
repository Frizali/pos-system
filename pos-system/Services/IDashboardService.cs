using pos_system.Models;

namespace pos_system.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> DashboardViewModel(string? fromDate, string? toDate);
    }
}
