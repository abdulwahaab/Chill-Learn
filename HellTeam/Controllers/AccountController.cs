using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;

namespace ChillLearn.Controllers
{
    public class AccountController : Controller
    {
        //public AccountController()
        //{
        //    this.userRepository = new UserRepository(new ChillLearnContext());
        //}

        //public AccountController(IUserRepository userRepository)
        //{
        //    this.userRepository = userRepository;
        //}

        // GET: Account
        public ActionResult register()
        {
            UserView userView = new UserView();
            userView.UserRoles = GetUserRoles();
            return View(userView);
        }

        public List<SelectListItem> GetUserRoles()
        {
            List<SelectListItem> userRoles = Enum.GetValues(typeof(UserRoles))
                                              .Cast<UserRoles>()
                                              .Select(t => new SelectListItem
                                              {
                                                  Value = Convert.ToInt16(t).ToString(),
                                                  Text = Enumerations.GetEnumDescription(t)
                                              }).ToList();

            return userRoles;
        }

        [HttpPost]
        public ActionResult register(UserView userView)
        {
            userView.UserRoles = GetUserRoles();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Please provide valid information.");        
                return View(userView);
            }
            UnitOfWork uow = new UnitOfWork();
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            string encryptedPassword = Encryptor.Encrypt(userView.Password);
            UserService us = new UserService();
            if (!us.DoesEmailExist(encryptedEmail))
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
                    Source = (int)SignupSource.App
                };
                uow.Users.Insert(user);
                uow.Save();
            }
            else
                ModelState.AddModelError("error", "Email address already exists, please use a different email.");
            return View(userView);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(LoginModel userView)
        {
            string encryptedEmail = Encryptor.Encrypt(userView.UserEmail);
            string encryptedPassword = Encryptor.Encrypt(userView.Password);
            UnitOfWork uow = new UnitOfWork();
            User user = uow.UserRepository.GetUserLogin(encryptedEmail, encryptedPassword,(int)SignupSource.App);
            Session["UserName"] = user.FirstName; /*for test app login*/
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public bool RegisterFacebook(UserView userView)
        {
            //userView.UserRoles = GetUserRoles();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Please provide valid information.");
                return false;
            }
            UnitOfWork uow = new UnitOfWork();
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            UserService us = new UserService();
            if (!us.DoesEmailExist(encryptedEmail))
            {
                User user = new User()
                {
                    UserID = Guid.NewGuid().ToString(),
                    FirstName = userView.FirstName,
                    LastName = userView.LastName,
                    CreationDate = DateTime.Now,
                    Email = encryptedEmail,
                    Status = (int)UserStatus.Pending,
                    Source = (int)SignupSource.Facebook
                };
                uow.Users.Insert(user);
                uow.Save();
            }
            else
                ModelState.AddModelError("error", "Email address already exists, please use a different email.");
            return true;
        }
        public bool LoginFacebook(UserView userView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Please provide valid information.");
                return false;
            }
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            UnitOfWork uow = new UnitOfWork();
            User user = uow.UserRepository.GetUserFacebookLogin(encryptedEmail, (int)SignupSource.Facebook);
            if (user != null)
            {
                Session["UserName"] = user.FirstName; /*for test fb login*/
                return true;
            }
            else { return false; }
           
        }
    }
}