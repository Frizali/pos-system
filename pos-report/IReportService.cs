namespace pos_report
{
    public interface IReportService
    {
        Stream GenerateReportPDF();
        void ExportXmlAndSchema();
    }
}
