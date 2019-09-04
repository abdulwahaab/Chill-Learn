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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Please provide valid information.");
                userView.UserRoles = GetUserRoles();
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
                    Status = (int)UserStatus.Pending
                };
                uow.Users.Insert(user);
                uow.Save();
            }
            else
                ModelState.AddModelError("error", "Email address already exists, please use a different email.");
            return View();
        }
    }
}