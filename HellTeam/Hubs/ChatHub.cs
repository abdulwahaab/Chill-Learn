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
        public void Send(string fromUser,string toUser, string message)
        {
            string connectionId = GetConnectionId(toUser);
            if (connectionId != "")
            {
                //Clients.All.addNewMessageToPage(fromUser, toUser, message);
                Clients.Client(connectionId).addNewMessageToPage(fromUser, toUser, message);
                Clients.Caller.addNewMessageToPage(fromUser, toUser, message);
            }
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
        public string GetConnectionId(string userId)
        {
            UnitOfWork uow = new UnitOfWork();
            User user = uow.Users.GetByID(userId);
            if (user != null)
            {
                return user.ConnectionId;
            }
            return "";
        }
    }
} 