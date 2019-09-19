using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChillLearn.DAL
{
    public class UserRepository
    {
        internal ChillLearnContext context;

        public UserRepository(ChillLearnContext context)
        {
            this.context = context;
        }
        public User GetUserLogin(string email,string password,int source,int status)
        {
            return context.Users.Where(u => u.Email == email && u.Password == password && u.Source == source && u.Status == status).FirstOrDefault();
        }
        public User GetUserFacebookLogin(string email, int source,int status)
        {
            return context.Users.Where(u => u.Email == email && u.Source == source && u.Status == status).FirstOrDefault();
        }
        public void SendEmail(string email,string subject,string emailHtml)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("diteeru44@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = emailHtml;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("diteeru44@gmail.com", "eruditetest");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                
            }
        }
        public bool UpdateUserStatus(string validationToken, int status)
        {
            try
            {
                var result = context.Users.SingleOrDefault(b => b.ValidationToken == validationToken);
                if (result != null)
                {
                    result.Status = status;
                    result.ValidationToken = "";
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ForgotPassword(string token,string email)
        {
            try
            {
                var result = context.Users.SingleOrDefault(b => b.Email == email);
                if (result != null)
                {
                    result.ValidationToken = token;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public User GetUserByToken(string validationToken)
        {
            try
            {
                var result = context.Users.SingleOrDefault(b => b.ValidationToken == validationToken);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool UpdadeUserPassword(string password,string token)
        {
            try
            {
                User result = context.Users.SingleOrDefault(b => b.ValidationToken == token);
                if(result != null)
                {
                    result.ValidationToken = "";
                    result.Password = password;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
