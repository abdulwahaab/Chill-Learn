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
    [AllowAnonymous]
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
            string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
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
                    ValidationToken = Token,
                    Source = (int)SignupSource.App
                };
                uow.Users.Insert(user);
                uow.Save();
                //send confirmation Email start
                var scheme = Request.Url.Scheme + "://";
                var host = Request.Url.Host + ":";
                var port = Request.Url.Port;
                string host1 = scheme + host + port;
                string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
                uow.UserRepository.SendEmail(userView.Email, "Chill Learn Email Confirmation", bodyHtml);
                //send confirmation Email end
                ModelState.AddModelError("success", "Successfully Registered!");
                TempData["Success"] = "Account created successfully, please check your inbox to verify your email address and continue to login.";
                return RedirectToAction("Login", "Account");

            }
            else
                ModelState.AddModelError("error", "Email address already exists, please use a different email.");
            return View(userView);
        }

        public ActionResult Login()
        {
            ViewBag.MessageSuccess = TempData["Success"];
            return View();
        }

        [HttpPost]
        public ActionResult login(LoginModel userView)
        {
            if (!ModelState.IsValid)
            {
                return View(userView);
            }
            else
            {
                string responseMsg = "An error occurred, please try again later.";
                string encryptedEmail = Encryptor.Encrypt(userView.UserEmail);
                string encryptedPassword = Encryptor.Encrypt(userView.Password);
                UnitOfWork uow = new UnitOfWork();
                User user = uow.UserRepository.GetUserLogin(encryptedEmail, encryptedPassword, (int)SignupSource.App);
                if (user != null)
                {
                    if (user.Status == (int)UserStatus.Approved)
                    {
                        SetLogin(user);
                        if (user.UserRole == (int)UserRoles.Student)
                        {
                            return RedirectToAction("Index", "Student");
                        }
                        else if (user.UserRole == (int)UserRoles.Teacher)
                        {
                            return RedirectToAction("Index", "Tutor");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else if (user.Status == (int)UserStatus.Pending)
                        responseMsg = "Please verify your email addresss by clicking the link sent to your email address.";
                    else if (user.Status == (int)UserStatus.Blocked)
                        responseMsg = "This account has been blocked, please contact support if you want to un-block your account.";
                    else if (user.Status == (int)UserStatus.Deleted)
                        responseMsg = "This account has been deleted.";
                }
                else
                {
                    responseMsg = "Please enter a valid email and password.";
                }
                ModelState.AddModelError("error", responseMsg);
                return View();
            }
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
                    Status = (int)UserStatus.Approved,
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
            User user = uow.UserRepository.GetUserFacebookLogin(encryptedEmail, (int)SignupSource.Facebook, (int)UserStatus.Approved);
            if (user != null)
            {
                SetLogin(user);
                return true;
            }
            else { return false; }

        }

        public ActionResult Email_Confirmation(string token)
        {
            if (token != null)
            {
                long TickDate = Convert.ToInt64(Encryptor.Decrypt(token));
                DateTime myDate = new DateTime(TickDate);
                if (DateTime.Now < myDate.AddDays(-1))
                {
                    ViewBag.Status = false;
                }
                else
                {
                    UnitOfWork uow = new UnitOfWork();
                    ViewBag.Status = uow.UserRepository.UpdateUserStatus(token, (int)UserStatus.Approved);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Forgot_Password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgot_Password(ForgotPasswordModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
                UnitOfWork uow = new UnitOfWork();
                bool result = uow.UserRepository.ForgotPassword(Token, Encryptor.Encrypt(user.UserEmail));
                if (result)
                {
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/reset_password?token=" + Token + "'>Click Here</a> to reset password </p>";
                    uow.UserRepository.SendEmail(user.UserEmail, "Chill Learn Recover Password", bodyHtml);
                    ModelState.AddModelError("success", "An Email sent to " + user.UserEmail + " please check email to reset password");
                }
                else
                {
                    ModelState.AddModelError("error", "Email does not exist");
                }
                return View();
            }
        }

        public ActionResult Reset_Password(string token)
        {
            UnitOfWork uow = new UnitOfWork();
            User user = uow.UserRepository.GetUserByToken(token);
            if (user != null)
            {
                ViewBag.Token = token;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Reset_Password(ResetPasswordModel pass)
        {
            if (!ModelState.IsValid)
            {
                return View(pass);
            }
            UnitOfWork uow = new UnitOfWork();
            bool result = uow.UserRepository.UpdadeUserPassword(Encryptor.Encrypt(pass.Password), pass.Token);
            if (result)
            {
                ModelState.AddModelError("success", "Password Successfully Changed");
            }
            else
            {
                ModelState.AddModelError("error", "Password Reset Failed");
            }
            return View();
        }

        public ActionResult Logoff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public void SetLogin(User user)
        {
            Session["UserName"] = user.FirstName;
            Session["UserRole"] = user.UserRole;
            Session["UserId"] = user.UserID;
            Session["Picture"] = user.Picture;
        }
    }
}