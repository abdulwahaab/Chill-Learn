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
            //if (Session["UserName"] != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("login", "account");
            //}
            return View();

        }
        //public ActionResult Dashboard()
        //{
        //    return View();
        //}
        //[Filters.AuthorizationFilter]
        //public ActionResult Profile()
        //{
        //    UserService userService = new UserService();
        //    User user = new User();
        //    var userId = Session["UserId"];
        //   //if((int)Session["UserRole"] == (int)UserRoles.Teacher)
        //   // {

        //   // }
        //   // else if((int)Session["UserRole"] == (int)UserRoles.Student)
        //   // {
        //       user  = userService.GetStudentProfile(userId.ToString());
        //    //}
        //    if (user != null)
        //    {
        //        ProfileModel profile = new ProfileModel
        //        {
        //            UserId = user.UserID,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            Email = Encryptor.Decrypt(user.Email),
        //            Picture = user.Picture,
        //            Address = user.Address,
        //            Country = user.Country,
        //            City = user.City,
        //            ProfileImage = user.Picture,
        //            //BirthDate = (DateTime)user.BirthDate,
        //            ContactNumber = user.ContactNumber

        //        };
        //        return View(profile);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //[HttpPost]
        //[Filters.AuthorizationFilter]
        //public ActionResult Profile(ProfileModel profile , HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        profile.ProfileImage = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
        //        string path = Path.Combine(Server.MapPath("~/Content/images/"), profile.ProfileImage);
        //        file.SaveAs(path);
        //        Session["Picture"] = profile.ProfileImage;
        //    }
        //        UnitOfWork uow = new UnitOfWork();
        //    UserService userService = new UserService();
        //    var userId = Session["UserId"].ToString();
        //    string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
        //    User user = userService.GetStudentProfile(userId);
        //    if (user != null)
        //    {
        //        if (user.Email != Encryptor.Encrypt(profile.Email))
        //        {
        //            var scheme = Request.Url.Scheme + "://";
        //            var host = Request.Url.Host + ":";
        //            var port = Request.Url.Port;
        //            string host1 = scheme + host + port;
        //            string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
        //            user.Status = (int)UserStatus.Pending;
        //            uow.UserRepository.SendEmail(profile.Email, "Chill Learn Recover Password", bodyHtml);
        //        }
        //        user.Email = Encryptor.Encrypt(profile.Email);
        //        user.FirstName = profile.FirstName;
        //        user.LastName = profile.LastName;
        //        user.Address = profile.Address;
        //        user.City = profile.City;
        //        user.Country = profile.Country;
        //        user.ContactNumber = profile.ContactNumber;
        //        user.UpdateDate = DateTime.Now;
        //        user.ValidationToken = Token;
        //        user.Picture = profile.ProfileImage;
        //        uow.Users.Update(user);
        //        uow.Save();

        //    }
        //    return View(profile);
        //}

    }
}