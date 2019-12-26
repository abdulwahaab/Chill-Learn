using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;

namespace ChillLearn.DAL
{
    public class BrainCert
    {
        public int CreateClassAsync(BrainCertClass classDetail)
        {
            using (var client = new HttpClient())
            {
                string braincertApiKey = ConfigurationManager.AppSettings.Get("BrainCertAPIKey");
                var response = client.PostAsync("https://api.braincert.com/v2/schedule?apikey="
                    + braincertApiKey + "&title=" + classDetail.Title + "&timezone=49&date=" + classDetail.Date
                    + "&start_time=" + classDetail.StartTime + "&end_time=" + classDetail.EndTime
                    + "& currency=SAR&ispaid=0&seat_attendees=1&record=" + classDetail.Record + "", null).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                if (!responseBody.Contains("error"))
                {
                    BrainCertClassSuccess classSuccess = JsonConvert.DeserializeObject<BrainCertClassSuccess>(responseBody);
                    return classSuccess.class_id;
                }
                else
                    return 0;
            }
        }
    }
}
