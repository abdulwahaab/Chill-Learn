using System;
using Facebook;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.ViewModels;
using ChillLearn.Data.Models;
using System.Collections.Generic;

namespace ChillLearn.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        private Uri RedirectUriLogin
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookLoginCallback");
                return uriBuilder.Uri;
            }
        }
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
            if (Session["UserId"] != null)
            {
                if (Convert.ToInt16(Session["UserRole"]) == (int)UserRoles.Student)
                    return RedirectToAction("classes", "student", null);
                else
                    return RedirectToAction("classes", "tutor", null);
            }
            else
            {
                ViewBag.MessageEmailExist = TempData["EmailExist"];
                UserView userView = new UserView();
                userView.UserRoles = GetUserRoles();
                return View(userView);
            }
        }

        public ActionResult Tutor()
        {
            if (Session["UserId"] != null)
            {
                if (Convert.ToInt16(Session["UserRole"]) == (int)UserRoles.Student)
                    return RedirectToAction("classes", "student", null);
                else
                    return RedirectToAction("classes", "tutor", null);
            }
            else
            {
                UnitOfWork uow = new UnitOfWork();
                ViewBag.Countries = uow.Countries.Get().ToList();
                ViewBag.Subjects = uow.Subjects.Get().ToList();
                ViewBag.Languages = GetLanguages();
                ViewBag.LanguageLevel = GetLanguageLevel();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Tutor(TutorRegistration userView, IEnumerable<HttpPostedFileBase> filesF)
        {
            UnitOfWork uow = new UnitOfWork();
            ViewBag.Countries = uow.Countries.Get().ToList();
            ViewBag.Subjects = uow.Subjects.Get().ToList();
            ViewBag.Languages = GetLanguages();
            ViewBag.LanguageLevel = GetLanguageLevel();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", Resources.Resources.ValidInfo);
                return View(userView);
            }
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            string encryptedPassword = Encryptor.Encrypt(userView.Password);
            string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
            UserService us = new UserService();
            if (!us.DoesEmailExist(encryptedEmail))
            {
                bool contactVerified = us.DoesContactNoExist(userView.ContactNumber);
                if (!contactVerified)
                {
                    User user = new User()
                    {
                        UserID = Guid.NewGuid().ToString(),
                        FirstName = userView.FirstName,
                        LastName = userView.LastName,
                        ContactNumber = userView.ContactNumber,
                        CreationDate = DateTime.Now,
                        Email = encryptedEmail,
                        Password = encryptedPassword,
                        UserRole = (int)UserRoles.Teacher,
                        Status = (int)UserStatus.Pending,
                        ValidationToken = Token,
                        Source = (int)SignupSource.App,
                        Picture = "NoImage.jpg",
                        Country = userView.Country
                    };

                    TeacherDetail teacherDetail = new TeacherDetail
                    {
                        TeacherID = user.UserID,
                        University = userView.University,
                        SubjectExperties = userView.Subject,
                        Qualification = userView.HigherQualification,
                        Description = userView.Description,
                        PreferedTime = userView.PreferedTime,
                        YearsExperience = userView.Experience,
                        CreationDate = DateTime.Now,
                        Status = 1
                    };

                    TeacherAccountDetail accountDetail = new TeacherAccountDetail
                    {
                        AccountName = userView.AccountHolder,
                        AccountNo = userView.AccountNo,
                        BranchCode = userView.BranchCode,
                        BranchName = userView.BranchName,
                        PinNo = userView.Pin,
                        Status = 1,
                        TeacherId = user.UserID
                    };
                    if (userView.SubjectTutored != null)
                    {
                        for (int i = 0; i < userView.SubjectTutored.Length; i++)
                        {
                            TeacherStage teacherStage = new TeacherStage
                            {
                                SubjectID = Convert.ToInt32(userView.SubjectTutored[i]),
                                TeacherID = user.UserID
                            };
                            uow.TeacherStages.Insert(teacherStage);
                        }
                    }

                    //Teacher Languages
                    //userView.Language = userView.Language.Remove(0, 1);
                    //string[] langs = Regex.Split(userView.Language, "-");
                    //if(langs != null)
                    //{
                    //    for (int i = 0; i < langs.Length; i++)
                    //    {
                    //        string[] langa = Regex.Split(langs[i], ",");
                    //        TeacherLanguage lang = new TeacherLanguage
                    //        {
                    //            Language = Convert.ToInt32(langa[0]),
                    //            LangLevel = Convert.ToInt32(langa[1]),
                    //            TeacherID = user.UserID
                    //        };
                    //        TeacherLanguage teacherLang = uow.TeacherLanguages.Get()
                    //            .Where(a => a.LangLevel == lang.LangLevel && a.Language == lang.Language && a.TeacherID == lang.TeacherID).FirstOrDefault();
                    //        if (teacherLang == null)
                    //        {
                    //            uow.TeacherLanguages.Insert(lang);
                    //        }
                    //    }
                    //}

                    TeacherLanguage lang = new TeacherLanguage
                    {
                        Language = Convert.ToInt32(userView.Language),
                        LangLevel = Convert.ToInt32(userView.LangLevel),
                        TeacherID = user.UserID
                    };
                    uow.TeacherLanguages.Insert(lang);

                    uow.Users.Insert(user);
                    uow.TeacherDetails.Insert(teacherDetail);
                    uow.TeacherAccountDetails.Insert(accountDetail);


                    if (filesF != null)
                    {
                        foreach (var file in filesF)
                        {
                            if (file != null)
                            {
                                string fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                                string path = Path.Combine(Server.MapPath("~/Content/images/teacher/"), fileName);
                                file.SaveAs(path);
                                TeacherFile teacherFile = new TeacherFile
                                {
                                    FileName = fileName,
                                    TeacherID = user.UserID,
                                    Type = 1
                                };
                                uow.TeacherFiles.Insert(teacherFile);
                            }
                        }
                    }
                    uow.Save();
                    //send notification to admin
                    Common.AddNotification(user.FirstName + " submitted his profile", "", user.UserID, "admin", "/admin/detail?id=" + user.UserID, (int)NotificationType.Teacher);
                    //send confirmation Email start
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string activationLink = "<a href='" + host1 + "/account/emailconfirmation?token=" + Token + "'>" + Resources.Resources.ClickHere + "</a>";
                    Utility.SendAccountActivationEmail(userView.Email, userView.FirstName, activationLink);
                    //send confirmation Email end
                    ModelState.AddModelError("success", Resources.Resources.AccountSuccess);
                    TempData["Success"] = Resources.Resources.AccountSuccess;
                    return RedirectToAction("Login", "Account");
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

        public List<SelectListItem> GetLanguages()
        {
            List<SelectListItem> languages = Enum.GetValues(typeof(Languages))
                                              .Cast<Languages>()
                                              .Select(t => new SelectListItem
                                              {
                                                  Value = Convert.ToInt16(t).ToString(),
                                                  Text = Enumerations.GetEnumDescription(t)
                                              }).ToList();

            return languages;
        }

        public List<SelectListItem> GetLanguageLevel()
        {
            List<SelectListItem> languageLevel = Enum.GetValues(typeof(LanguageLevel))
                                              .Cast<LanguageLevel>()
                                              .Select(t => new SelectListItem
                                              {
                                                  Value = Convert.ToInt16(t).ToString(),
                                                  Text = Enumerations.GetEnumDescription(t)
                                              }).ToList();

            return languageLevel;
        }

        [HttpPost]
        public ActionResult register(UserView userView)
        {
            userView.UserRoles = GetUserRoles();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", Resources.Resources.MsgPleaseProvideValid);
                return View(userView);
            }
            UnitOfWork uow = new UnitOfWork();
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            string encryptedPassword = Encryptor.Encrypt(userView.Password);
            string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
            UserService us = new UserService();
            if (!us.DoesEmailExist(encryptedEmail))
            {
                //bool contactVerified = string.IsNullOrEmpty(userView.ContactNumber) ? true : us.DoesContactNoExist(userView.ContactNumber);
                bool contactVerified = us.DoesContactNoExist(userView.ContactNumber);
                if (!contactVerified)
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
                        Source = (int)SignupSource.App,
                        Picture = "NoImage.jpg"
                    };
                    uow.Users.Insert(user);
                    uow.Save();
                    //send confirmation Email start
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/emailconfirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
                    uow.UserRepository.SendEmail(userView.Email, "Chill Learn Email Confirmation", bodyHtml);
                    //send confirmation Email end
                    ModelState.AddModelError("success", "Successfully Registered!");
                    TempData["Success"] = Resources.Resources.MsgAccountCreatedSuccess;
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("error", Resources.Resources.MsgContactAlreadyExist);
                }

            }
            else
            {
                ModelState.AddModelError("error", Resources.Resources.MsgEmailAlreadyExist);
            }
            return View(userView);
        }

        public ActionResult Login(string returnurl)
        {
            if (Session["UserId"] != null)
            {
                if (Convert.ToInt16(Session["UserRole"]) == (int)UserRoles.Student)
                    return RedirectToAction("classes", "student", null);
                else
                    return RedirectToAction("classes", "tutor", null);
            }
            else
            {
                ViewBag.MessageSuccess = TempData["Success"];
                TempData["ReturnUrl"] = returnurl;
                return View();
            }
        }

        [HttpPost]
        public ActionResult login(LoginModel userView)
        {
            //string returnurl = TempData["ReturnUrl"].ToString();
            string responseMsg = Resources.Resources.MsgProvideLoginDetail;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", responseMsg);
                return View(userView);
            }
            else
            {
                responseMsg = Resources.Resources.MsgErrorTryAgain;
                string encryptedEmail = Encryptor.Encrypt(userView.UserEmail);
                string encryptedPassword = Encryptor.Encrypt(userView.Password);
                UnitOfWork uow = new UnitOfWork();
                User user = uow.UserRepository.GetUserLogin(encryptedEmail, encryptedPassword, (int)SignupSource.App);
                if (user != null)
                {
                    if (user.Status != (int)UserStatus.Pending && user.Status != (int)UserStatus.Blocked && user.Status != (int)UserStatus.Deleted)
                    {
                        SetLogin(user);
                        if (TempData["ReturnUrl"] == null)
                        {
                            if (user.UserRole == (int)UserType.Student)
                            {
                                return RedirectToAction("index", "student");
                            }
                            else if (user.UserRole == (int)UserType.Teacher)
                            {
                                return RedirectToAction("profile", "tutor");
                            }
                            else if (user.UserRole == (int)UserType.Admin)
                            {
                                return RedirectToAction("index", "admin");
                            }
                            else
                            {
                                return RedirectToAction("index", "home");
                            }
                        }
                        else
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                    }
                    else if (user.Status == (int)UserStatus.Pending)
                        responseMsg = Resources.Resources.MsgVerifyEmail;
                    else if (user.Status == (int)UserStatus.Blocked)
                        responseMsg = Resources.Resources.MsgAccountBlocked;
                    else if (user.Status == (int)UserStatus.Deleted)
                        responseMsg = Resources.Resources.MsgAccountDeleted;
                }
                else
                {
                    responseMsg = Resources.Resources.MsgEnterValidEmailPass;
                }
            }
            ModelState.AddModelError("error", responseMsg);
            return View();
        }

        [HttpPost]
        public bool RegisterFacebook(UserView userView)
        {
            //userView.UserRoles = GetUserRoles();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", Resources.Resources.MsgPleaseProvideValid);
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
                ModelState.AddModelError("error", Resources.Resources.MsgEmailAlreadyExist);
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

        public ActionResult EmailConfirmation(string token)
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
                    ViewBag.Status = uow.UserRepository.UpdateUserStatus(token, (int)UserStatus.Verified);
                }
                return View();
            }
            else
            {
                return RedirectToAction("index", "home");
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
                    ModelState.AddModelError("success", Resources.Resources.MsgSuccessP1 + " " + user.UserEmail + " " + Resources.Resources.MsgSuccessP2);
                }
                else
                {
                    ModelState.AddModelError("error", Resources.Resources.MsgEmailDoesNotExist);
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
            return RedirectToAction("index", "home");
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
                TempData["Success"] = Resources.Resources.MsgPasswordChangedSuccess;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError("error", Resources.Resources.MsgResetFail);
            }
            return View();
        }

        public ActionResult Logoff()
        {
            Session.Abandon();
            return RedirectToAction("index", "home");
        }

        public void SetLogin(User user)
        {
            Session["UserName"] = user.FirstName;
            Session["UserRole"] = user.UserRole;
            Session["UserId"] = user.UserID;
            Session["Picture"] = user.Picture;
            Session["UserStatus"] = user.Status;
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "565023847660764",
                client_secret = "1751815d3edd8a071e2eceb6da137618",
                //client_id = "2354798001501872",
                //client_secret = "bf438a1805bbdd1543551775850df272",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                //client_id = "2354798001501872",
                //client_secret = "bf438a1805bbdd1543551775850df272",
                client_id = "565023847660764",
                client_secret = "1751815d3edd8a071e2eceb6da137618",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;

            // Store the access token in the session for farther use
            //Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information, like email, first name, middle name etc
            dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
            string email = me.email;
            string firstname = me.first_name;
            string middlename = me.middle_name;
            string lastname = me.last_name;

            if (!string.IsNullOrEmpty(email))
            {
                string encryptedEmail = Encryptor.Encrypt(email);
                UserService us = new UserService();
                if (!us.DoesEmailExist(encryptedEmail))
                {
                    User user = new User()
                    {
                        UserID = Guid.NewGuid().ToString(),
                        FirstName = firstname,
                        LastName = lastname,
                        CreationDate = DateTime.Now,
                        Email = encryptedEmail,
                        UserRole = (int)UserRoles.Student,
                        Status = (int)UserStatus.Verified,
                        Source = (int)SignupSource.Facebook,
                        Picture = "NoImage.jpg"
                    };
                    UnitOfWork uow = new UnitOfWork();
                    uow.Users.Insert(user);
                    uow.Save();
                }
                else
                {
                    TempData["EmailExist"] = "Email Already Exist";
                    return RedirectToAction("register", "account");
                }
            }
            TempData["Success"] = Resources.Resources.MsgFbRegisterSuccess;
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult FacebookLogin()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "565023847660764",
                client_secret = "1751815d3edd8a071e2eceb6da137618",
                //client_id = "2354798001501872",
                //client_secret = "bf438a1805bbdd1543551775850df272",
                redirect_uri = RedirectUriLogin.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookLoginCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                //client_id = "2354798001501872",
                //client_secret = "bf438a1805bbdd1543551775850df272",
                client_id = "565023847660764",
                client_secret = "1751815d3edd8a071e2eceb6da137618",
                redirect_uri = RedirectUriLogin.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=first_name,email");
            string email = me.email;
            string responseMsg = Resources.Resources.MsgProvideLoginDetail;
            if (!string.IsNullOrEmpty(email))
            {
                responseMsg = Resources.Resources.MsgErrorTryAgain;
                string encryptedEmail = Encryptor.Encrypt(email);
                UnitOfWork uow = new UnitOfWork();
                User user = uow.UserRepository.GetUserFacebookLogin(encryptedEmail, (int)SignupSource.Facebook, (int)UserStatus.Verified);
                if (user != null)
                {
                    if (user.Status != (int)UserStatus.Pending && user.Status != (int)UserStatus.Blocked && user.Status != (int)UserStatus.Deleted)
                    {
                        SetLogin(user);
                        if (user.UserRole == (int)UserType.Student)
                        {
                            return RedirectToAction("index", "student");
                        }
                        else if (user.UserRole == (int)UserType.Teacher)
                        {
                            return RedirectToAction("profile", "tutor");
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                    else if (user.Status == (int)UserStatus.Pending)
                        responseMsg = Resources.Resources.MsgVerifyEmail;
                    else if (user.Status == (int)UserStatus.Blocked)
                        responseMsg = Resources.Resources.MsgAccountBlocked;
                    else if (user.Status == (int)UserStatus.Deleted)
                        responseMsg = Resources.Resources.MsgAccountDeleted;
                }
                else
                {
                    responseMsg = Resources.Resources.MsgEnterValidEmailPass;
                }
            }
            ModelState.AddModelError("error", responseMsg);
            return View("login");
        }
    }
}