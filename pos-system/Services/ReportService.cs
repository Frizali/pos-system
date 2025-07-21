using Newtonsoft.Json;
using pos_system.Models;
using System.Net.Http.Headers;
using System.Text;

namespace pos_system.Services
{
    public class ReportService : IReportService
    {
        public async Task<ReportModel> GenerateReportPDF(ReportParamModel param)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.7:5001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(param);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("report", content);


                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReportModel>(responseJson) ?? throw new ApplicationException("Error while generate report");
                }
                else
                {
                    throw new ApplicationException("Error while generate report");
                }
            }
        }
    }
}
