﻿using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeTeacher]
    public class TutorController : Controller
    {
        // GET: Tutor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Applications()
        {
            return View();
        }

        public ActionResult Payment_Roll()
        {
            return View();
        }
        public ActionResult Payment_History()
        {
            return View();
        }
        public ActionResult Tutor_Profile()
        {
            return View();
        }

        public ActionResult Search_Question()
        {
            UnitOfWork uow = new UnitOfWork();
            
            List<StudentProblemsModel> problems = uow.UserRepository.GetProblems(Session["UserId"].ToString());
            return View(problems);
        }
        public ActionResult question_detail(string q)
        {
            if (q != null)
            {
                UnitOfWork uow = new UnitOfWork();
                QuestionDetailModel model = new QuestionDetailModel();
                model.QuestionDetail = uow.UserRepository.GetQuestionDetailById(q);
                if (model.QuestionDetail != null)
                {
                    model.ProblemId = q;
                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult PostBid(QuestionDetailModel model)
        {
            UnitOfWork uow = new UnitOfWork();

            if (!ModelState.IsValid)
            {
                model.QuestionDetail = uow.UserRepository.GetQuestionDetailById(model.ProblemId);
                //return View(model);
                return View("question_detail", model);
            }
            StudentProblemBid problem = new StudentProblemBid
            {
                BidID = Guid.NewGuid().ToString(),
                UserID = Session["UserId"].ToString(),
                ProblemID = model.ProblemId,
                CreationDate = DateTime.Now,
                Description = model.Response,
                Status = (int)BidStatus.Pending,
            };
            uow.StudentProblemBids.Insert(problem);
            uow.Save();
            return RedirectToAction("search_question", "tutor");
        }

        public ActionResult Bids()
        {
            UnitOfWork uow = new UnitOfWork();
            List<BidsModel> model = uow.UserRepository.GetBidsByUserId(Session["UserId"].ToString());
            return View(model);
        }

        public ActionResult Profile()
        {
            UserService userService = new UserService();
            User user = new User();
            var userId = Session["UserId"];
            //if((int)Session["UserRole"] == (int)UserRoles.Teacher)
            // {

            // }
            // else if((int)Session["UserRole"] == (int)UserRoles.Student)
            // {
            user = userService.GetStudentProfile(userId.ToString());
            //}
            if (user != null)
            {
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
                    ProfileImage = user.Picture,
                    //BirthDate = (DateTime)user.BirthDate,
                    ContactNumber = user.ContactNumber

                };
                return View(profile);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Filters.AuthorizationFilter]
        public ActionResult Profile(ProfileModel profile, HttpPostedFileBase file)
        {
            UserService userService = new UserService();
            var userId = Session["UserId"].ToString();
            User user = userService.GetStudentProfile(userId);
            if (file != null)
            {
                profile.ProfileImage = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), profile.ProfileImage);
                file.SaveAs(path);
                Session["Picture"] = profile.ProfileImage;
                user.Picture = profile.ProfileImage;
            }
            UnitOfWork uow = new UnitOfWork();

            if (user != null)
            {
                if (user.Email != Encryptor.Encrypt(profile.Email))
                {
                    string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
                    user.ValidationToken = Token;
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
                uow.Users.Update(user);
                uow.Save();

            }
            return View(profile);
        }
        public ActionResult Classes()
        {
            UnitOfWork uow = new UnitOfWork();
            List<ClassesModel> model = uow.TeacherRepository.GetClasses(Session["UserId"].ToString());
            return View(model);
        }
    }
}