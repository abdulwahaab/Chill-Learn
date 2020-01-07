using System;
using System.Collections.Generic;
using System.Linq;
using ChillLearn.Data.Models;

namespace ChillLearn.DAL
{
    public class Common
    {
        public static void AddNotification(string title, string desc, string fromUser, string toUser, string url, int type)
        {
            UnitOfWork uow = new UnitOfWork();
            Notification notification = new Notification();
            notification.Title = title.Trim();
            notification.Description = desc.Trim();
            notification.FromUser = fromUser;
            notification.ToUser = toUser;
            notification.URL = url;
            notification.CreationDate = DateTime.Now;
            notification.IsRead = false;
            notification.Type = type;
            uow.Notifications.Insert(notification);
            uow.Save();
        }

        public string MarkNotificationRead(int id)
        {
            UnitOfWork uow = new UnitOfWork();
            Notification notification = uow.Notifications.GetByID(id);
            notification.IsRead = true;
            notification.ReadingDate = DateTime.Now;
            uow.Save();
            return notification.URL;
        }

        public static bool UserHasCredits(string userId, decimal creditCheck)
        {
            UnitOfWork uow = new UnitOfWork();
            StudentCredit credit = uow.StudentCredits.Get(x => x.StudentID == userId).FirstOrDefault();
            uow.Dispose();
            return credit.TotalCredits > creditCheck;
        }

        public List<string> GetHours()
        {
            List<string> hoursList = new List<string>();
            hoursList.Add("01");
            hoursList.Add("02");
            hoursList.Add("03");
            hoursList.Add("04");
            hoursList.Add("05");
            hoursList.Add("06");
            hoursList.Add("07");
            hoursList.Add("08");
            hoursList.Add("09");
            hoursList.Add("10");
            hoursList.Add("11");
            hoursList.Add("12");
            return hoursList;
        }

        public List<string> GetDurationHours()
        {
            List<string> hoursList = new List<string>();
            hoursList.Add("0");
            hoursList.Add("1");
            hoursList.Add("2");
            hoursList.Add("3");
            hoursList.Add("4");
            hoursList.Add("5");
            return hoursList;
        }

        public List<string> GetMinutes()
        {
            List<string> minutesList = new List<string>();
            minutesList.Add("00");
            minutesList.Add("05");
            minutesList.Add("10");
            minutesList.Add("15");
            minutesList.Add("20");
            minutesList.Add("25");
            minutesList.Add("30");
            minutesList.Add("35");
            minutesList.Add("40");
            minutesList.Add("45");
            minutesList.Add("50");
            minutesList.Add("55");
            return minutesList;
        }

        public List<string> GetAMPM()
        {
            List<string> resultList = new List<string>();
            resultList.Add("AM");
            resultList.Add("PM");
            return resultList;
        }

        public List<string> GetYearsList()
        {
            List<string> resultList = new List<string>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear; i > currentYear - 50; i--)
            {
                resultList.Add(i.ToString());
            }
            return resultList;
        }
    }
}
