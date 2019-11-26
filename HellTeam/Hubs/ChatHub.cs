using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using Microsoft.AspNet.SignalR;

namespace ChillLearn
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }
        public void UpdateConnectionId(string connectionId,string userId)
        {
            UnitOfWork uow = new UnitOfWork();
            User user = uow.Users.GetByID(userId);
            if(user != null)
            {
                user.ConnectionId = connectionId;
                uow.Users.Update(user);
                uow.Save();
            }
        }
    }
} 