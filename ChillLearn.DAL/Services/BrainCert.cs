using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using System;

namespace ChillLearn.DAL
{
    public class BrainCert
    {
        public int CreateClass(BrainCertClass classDetail, int timeZone)
        {
            using (var client = new HttpClient())
            {
                string braincertApiKey = ConfigurationManager.AppSettings.Get("BrainCertAPIKey");
                var response = client.PostAsync("https://api.braincert.com/v2/schedule?apikey="
                    + braincertApiKey + "&title=" + classDetail.Title + "&timezone=" + timeZone + "&date=" + classDetail.Date
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

        public string GetLaunchURL(int classId, int userId, string userName, string className, string courseName, bool isTeacher)
        {
            using (var client = new HttpClient())
            {
                string braincertApiKey = ConfigurationManager.AppSettings.Get("BrainCertAPIKey");
                var response = client.PostAsync("https://api.braincert.com/v2/getclasslaunch?apikey=" + braincertApiKey + "&class_id=" + classId +
                    "&userId=" + userId + "&userName=" + userName + "&isTeacher=" + Convert.ToInt16(isTeacher) +
                    "&courseName=" + courseName + "&lessonName=" + className, null).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                if (!responseBody.Contains("error"))
                {
                    BrainCertClassURL classURL = JsonConvert.DeserializeObject<BrainCertClassURL>(responseBody);
                    return classURL.launchurl;
                }
                else
                    return "";
            }
        }

        public static int CreateBrainCertClass(string title, string classDate, string startTime, string endTime, int record, int timeZone)
        {
            string[] dateArray = classDate.Split(' ')[0].Split('/');
            string properDate = dateArray[2] + "-" + dateArray[0] + "-" + dateArray[1];
            BrainCert bc = new BrainCert();
            BrainCertClass bClass = new BrainCertClass();
            bClass.Title = title;
            bClass.Date = properDate;
            bClass.StartTime = startTime;
            bClass.EndTime = endTime;
            bClass.Record = record;
            return bc.CreateClass(bClass, timeZone);
        }
    }
}
