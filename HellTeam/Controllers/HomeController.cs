using ChillLearn.DAL;
using ChillLearn.CustomModels;
using ChillLearn.DAL.Services;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;

// to work with Resources file you should add these library
//using System.Windows.Forms;
//using System.Resources;
//using System.Configuration;
//using System.IO;
//using System.Reflection;
//using System.Globalization;

namespace ChillLearn.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserView userView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Please provide valid information.");
                return View(userView);
            }
            UnitOfWork uow = new UnitOfWork();
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            string encryptedPassword = Encryptor.Encrypt(userView.Password);
            string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
            UserService us = new UserService();
            if (!us.DoesEmailExist(encryptedEmail))
            {
                if (!us.DoesContactNoExist(userView.ContactNumber))
                {
                    User user = new User()
                    {
                        UserID = Guid.NewGuid().ToString(),
                        FirstName = userView.FirstName,
                        LastName = userView.LastName,
                        Class = userView.Class,
                        ContactNumber = userView.ContactNumber,
                        CreationDate = DateTime.Now,
                        Email = encryptedEmail,
                        Grade = userView.Grade,
                        Password = encryptedPassword,
                        UserRole = userView.UserRole,
                        Status = (int)UserStatus.Pending,
                        ValidationToken = Token,
                        Source = (int)SignupSource.App
                    };
                    uow.Users.Insert(user);
                    uow.Save();
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
                    uow.UserRepository.SendEmail(userView.Email, "Chill Learn Email Confirmation", bodyHtml);
                    ViewBag.Message = "Account created successfully, please check your inbox to verify your email address.";
                    return View(userView);
                }
                else
                {
                    ModelState.AddModelError("error", "Contact number already exists, please use a different Contact number.");
                }
            }
            else
            {
                ModelState.AddModelError("error", "Email address already exists, please use a different email.");
            }
            return View(userView);
        }

        public ActionResult main()
        {
            return View();
        }
        [Filters.AuthorizationFilter]
        public ActionResult search(int p)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.GetTutorBySubject(p);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSubjects(string name)
        {
            UnitOfWork uow = new UnitOfWork();
            var list = uow.Subjects.Get().Where(x => x.SubjectName.ToLower().Contains(name.ToLower())).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}