using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;

namespace ChillLearn.DAL
{
    public class BrainCert
    {
        public async Task<string> CreateClassAsync(BrainCertClass classDetail)
        {
            UnitOfWork uow = new UnitOfWork();
            using (var client = new HttpClient())
            {
                string braincertApiKey = ConfigurationManager.AppSettings.Get("BrainCertAPIKey");
                var response = await client.PostAsync("https://api.braincert.com/v2/schedule?apikey="
                    + braincertApiKey + "&title=" + classDetail.Title + "&timezone=73&date=" + classDetail.Date
                    + "&start_time=" + classDetail.StartTime + "&end_time=" + classDetail.EndTime
                    + "& currency=SAR&ispaid=0&seat_attendees=1&record=" + classDetail.IsRecord + "", null);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!responseBody.Contains("error"))
                {
                    BrainCertClassSuccess classSuccess = JsonConvert.DeserializeObject<BrainCertClassSuccess>(responseBody);
                    return classSuccess.class_id;
                }
                else
                    return "error";
            }
        }
    }
}
