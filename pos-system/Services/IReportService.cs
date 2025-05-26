using pos_system.Models;

namespace pos_system.Services
{
    public interface IReportService
    {
        Task<ReportModel> GenerateReportPDF(ReportParamModel param);
    }
}
