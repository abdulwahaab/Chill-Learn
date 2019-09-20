using ChillLearn.DAL;
using ChillLearn.DAL.Services;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeStudent]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Refund_Request()
        {
            return View();
        }
        public ActionResult Student_Profile()
        {
            StudentService studentService = new StudentService();
            var userId = Session["UserId"].ToString();
            User user = studentService.GetProfile(userId);
            ProfileModel profile = new ProfileModel
            {
                UserId = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = Encryptor.Decrypt(user.Email),
                Picture = user.Picture,
                Address = user.Address,
                Country = user.Country,
                City = user.City,
                //BirthDate = (DateTime)user.BirthDate,
                ContactNumber = user.ContactNumber

            };
            return View(profile);
        }
        [HttpPost]
        public ActionResult Student_Profile(ProfileModel profile)
        {
            UnitOfWork uow = new UnitOfWork();
            StudentService studentService = new StudentService();
            var userId = Session["UserId"].ToString();
            string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
            User user = studentService.GetProfile(userId);
            if (user != null)
            {
                if (user.Email != Encryptor.Encrypt(profile.Email))
                {
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
                    user.Status = (int)UserStatus.Pending;
                    uow.UserRepository.SendEmail(profile.Email, "Chill Learn Recover Password", bodyHtml);
                }
                user.Email = Encryptor.Encrypt(profile.Email);
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.Address = profile.Address;
                user.City = profile.City;
                user.Country = profile.Country;
                user.ContactNumber = profile.ContactNumber;
                user.UpdateDate = DateTime.Now;
                user.ValidationToken = Token;
                uow.Users.Update(user);
                uow.Save();

            }
                    return View();
        }
        }
}