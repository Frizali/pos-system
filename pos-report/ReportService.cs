using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace pos_report
{
    public class ReportService
    {
        private readonly string _connectionString = "server=(localdb)\\MSSQLLocalDB;database=POS;Trusted_Connection=true;TrustServerCertificate=True;";

        public Stream GenerateReportPDF()
        {
            var ds = new DataSet();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string queryHeader = "SELECT 'Angkringan OmahMU' AS Owner, '2025-05-01' AS FromDate, '2025-05-31' AS ToDate";
                SqlDataAdapter daHeader = new SqlDataAdapter(queryHeader, con);
                daHeader.Fill(ds, "Header");

                string query = "SELECT CONVERT(date, OrderDate) AS DateID, SUM(TotalPrice) AS TotalAmount, SUM(Quantity) AS TotalProductSales FROM tblOrder o LEFT JOIN tblOrderItem oi ON o.OrderID = oi.OrderID GROUP BY CONVERT(date, OrderDate)";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(ds, "SalesAnalytics");
            }

            ReportDocument rptDoc = new ReportDocument();
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "SalesAnalytics.rpt");
            rptDoc.Load(reportPath);
            rptDoc.SetDataSource(ds);

            return rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
        }

        public void ExportXmlAndSchema()
        {
            var ds = new DataSet();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string queryHeader = "SELECT 'Angkringan OmahMU' AS Owner, '2025-05-01' AS FromDate, '2025-05-31' AS ToDate";
                SqlDataAdapter daHeader = new SqlDataAdapter(queryHeader, con);
                daHeader.Fill(ds, "Header");

                string query = "SELECT CONVERT(date, OrderDate) AS DateID, SUM(TotalPrice) AS TotalAmount, SUM(Quantity) AS TotalProductSales FROM tblOrder o LEFT JOIN tblOrderItem oi ON o.OrderID = oi.OrderID GROUP BY CONVERT(date, OrderDate)";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(ds, "SalesAnalytics");
            }

            ds.WriteXml("D:\\pos-report\\SalesAnalytics.xml", XmlWriteMode.WriteSchema);
            ds.WriteXmlSchema("D:\\pos-report\\SalesAnalytics.xsd");
        }
    }
}
