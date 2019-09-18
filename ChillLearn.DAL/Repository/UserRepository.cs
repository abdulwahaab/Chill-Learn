using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.DAL
{
    public class UserRepository
    {
        internal ChillLearnContext context;

        public UserRepository(ChillLearnContext context)
        {
            this.context = context;
        }
        public User GetUserLogin(string email,string password,int source)
        {
            return context.Users.Where(u => u.Email == email && u.Password == password && u.Source == source).FirstOrDefault();
        }
        public User GetUserFacebookLogin(string email, int source)
        {
            return context.Users.Where(u => u.Email == email && u.Source == source).FirstOrDefault();
        }
    }
}
