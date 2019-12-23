using System;
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
    }
}
