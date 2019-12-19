using ChillLearn.Data.Models;
using System;

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
    }
}
