using Moq;
using pos_report;

namespace pos_test
{
    class ReportTest
    {

        [Test]
        public async Task Create_XMLReport_ShouldSaved()
        {
            var reportService = new ReportService();

            reportService.ExportXmlAndSchema();
        }

        [Test]
        public async Task Create_PDFReport_ShouldReturnStream()
        {
            var reportService = new ReportService();
            var stream = reportService.GenerateReportPDF();
        }
    }
}