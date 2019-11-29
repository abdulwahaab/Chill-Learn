using ChillLearn.DAL;
using ChillLearn.CustomModels;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using ChillLearn.Helpers;

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
                ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
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
                    string activationLink = "<a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>" + Resources.Resources.ClickHere + "</a>";
                    Utility.SendAccountActivationEmail(userView.Email, userView.FirstName, activationLink);
                    ViewBag.Message = Resources.Resources.AccountSuccess;
                    return View(userView);
                }
                else
                {
                    ModelState.AddModelError("error", Resources.Resources.PhoneExists);
                }
            }
            else
            {
                ModelState.AddModelError("error", Resources.Resources.EmailAlreadyExists);
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

        [Filters.AuthorizationFilter]
        public ActionResult Profile(string p)
        {
            UnitOfWork uow = new UnitOfWork();
            TeacherProfileView profileView = new TeacherProfileView();
            if (!string.IsNullOrEmpty(p))
            {
                profileView.Profile = uow.TeacherRepository.GetTeacherProfile(p);
                string email = Encryptor.Decrypt(profileView.Profile.Email);
                profileView.Profile.Email = email;
                profileView.Subjects = uow.TeacherRepository.GetTeacherStages(p);
                profileView.Education = uow.TeacherQualifications.Get().Where(a => a.TeacherID == p).ToList();
            }
            return View(profileView);
        }

        [HttpGet]
        public JsonResult GetSubjects(string name)
        {
            UnitOfWork uow = new UnitOfWork();
            var list = uow.Subjects.Get().Where(x => x.SubjectName.ToLower().Contains(name.ToLower())).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChnageLang(string lang)
        {
            Session["lang"] = lang;
            return Json("true", JsonRequestBehavior.AllowGet);
        }
    }
}