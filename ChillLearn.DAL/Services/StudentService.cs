using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.DAL.Services
{
    public class StudentService
    {
        public User GetProfile(string userId)
        {
            UnitOfWork uow = new UnitOfWork();
            User result = uow.Users.Get(x => x.UserID == userId).FirstOrDefault();
            return result;
        }


    }
}
