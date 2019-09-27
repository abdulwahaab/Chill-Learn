using System;
using System.Linq;
using System.Data.Entity;
using ChillLearn.Data.Models;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ChillLearn.DAL
{
    public class UserService
    {
        public bool DoesEmailExist(string email)
        {
            UnitOfWork uow = new UnitOfWork();
            var result = uow.Users.Get(x => x.Email == email).ToList();
            if (result.Count > 0)
                return true;
            else
                return false;
        }

        public User GetStudentProfile(string userId)
        {
            UnitOfWork uow = new UnitOfWork();
            User result = uow.Users.Get(x => x.UserID == userId).FirstOrDefault();
            return result;
        }


    }
}
