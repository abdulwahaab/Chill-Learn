﻿using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.DAL.Services;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeStudent]
    public class StudentController : BaseController
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

        public List<SelectListItem> GetSessionTypess()
        {
            List<SelectListItem> userRoles = Enum.GetValues(typeof(SessionType))
                                              .Cast<SessionType>()
                                              .Select(t => new SelectListItem
                                              {
                                                  Value = Convert.ToInt16(t).ToString(),
                                                  Text = Enumerations.GetEnumDescription(t)
                                              }).ToList();

            return userRoles;
        }

        public ActionResult CreateProblem(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            ProblemsModel model = new ProblemsModel();
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            model.SessionTypes = GetSessionTypess();
            model.TeacherID = id;
            if (!string.IsNullOrEmpty(id))
            {
                User selectedTeacher = uow.Users.GetByID(id);
                ViewBag.TeacherName = selectedTeacher.FirstName + " " + selectedTeacher.LastName;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateProblem(ProblemsModel model, HttpPostedFileBase file)
        {
            string userId = Session["UserId"].ToString();
            UnitOfWork uow = new UnitOfWork();
            decimal balanceHours = (decimal)(uow.StudentCredits.Get(x => x.StudentID == userId).FirstOrDefault().TotalCredits);
            if (balanceHours > model.HoursNeeded)
            {
                model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                model.SessionTypes = GetSessionTypess();
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
                    return View(model);
                }
                string fileName = null;
                if (file != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    file.SaveAs(path);
                }
                StudentProblem problem = new StudentProblem
                {
                    ProblemID = Guid.NewGuid().ToString(),
                    StudentID = Session["UserId"].ToString(),
                    SubjectID = model.Subject,
                    CreationDate = DateTime.Now,
                    Description = model.ProblemDescription,
                    HoursNeeded = model.HoursNeeded,
                    Type = model.Type,
                    FileName = fileName,
                    TeacherID = model.TeacherID,
                    Status = model.TeacherID != null ? (int)ProblemStatus.TeacherSelected : (int)ProblemStatus.Created,
                    ExpireDate = DateTime.ParseExact(model.DeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture) // need to add datetime datepicker
                };
                uow.StudentProblems.Insert(problem);
                uow.Save();
                //add notification if teacher is selected
                if (!string.IsNullOrEmpty(model.TeacherID))
                    Common.AddNotification(Session["UserName"] + " asked you a question", "", Session["UserId"].ToString(), model.TeacherID, "/tutor/writeproposal?q=" + problem.ProblemID, (int)NotificationType.Question);
                //
                ModelState.AddModelError("success", Resources.Resources.MsgProblemSubmitedSuccessfully);
                return View(model);
            }
            else
            {
                model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                model.SessionTypes = GetSessionTypess();
                ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
                return View(model);
            }
        }

        public ActionResult Problems()
        {
            UnitOfWork uow = new UnitOfWork();
            ProblemsModel model = new ProblemsModel();
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            return View(model);
        }

        [HttpPost]
        public ActionResult Problems(ProblemsModel model, HttpPostedFileBase file)
        {
            UnitOfWork uow = new UnitOfWork();
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypess();
            if (!ModelState.IsValid)
            {
                model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
                ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
                return View(model);
            }
            string fileName = null;
            if (file != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                file.SaveAs(path);
                //byte[] bytes;
                //using (BinaryReader br = new BinaryReader(file.InputStream))
                //{
                //    bytes = br.ReadBytes(file.ContentLength);
                //}
                //fileName = bytes;

            }
            StudentProblem problem = new StudentProblem
            {
                ProblemID = Guid.NewGuid().ToString(),
                StudentID = Session["UserId"].ToString(),
                SubjectID = model.Subject,
                CreationDate = DateTime.Now,
                Description = model.ProblemDescription,
                HoursNeeded = model.HoursNeeded,
                Type = model.Type,
                FileName = fileName,
                ExpireDate = DateTime.ParseExact(model.DeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture) // need to add datetime datepicker
            };
            uow.StudentProblems.Insert(problem);
            uow.Save();
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            ModelState.AddModelError("success", Resources.Resources.MsgProblemSubmitedSuccessfully);
            return View(model);
        }

        public ActionResult Problem_Detail(string problem)
        {
            if (problem != null)
            {
                UnitOfWork uow = new UnitOfWork();
                ProblemDetailModel model = new ProblemDetailModel();
                model.ProblemDetails = uow.UserRepository.GetQuestionDetailById(problem);
                if (model.ProblemDetails != null)
                {
                    model.ProblemId = problem;
                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Problem_Detail(ProblemDetailModel model)
        {
            UnitOfWork uow = new UnitOfWork();

            if (!ModelState.IsValid)
            {
                model.ProblemDetails = uow.UserRepository.GetQuestionDetailById(model.ProblemId);
                return View(model);
            }
            StudentProblemBid problem = new StudentProblemBid
            {
                BidID = Guid.NewGuid().ToString(),
                UserID = Session["UserId"].ToString(),
                ProblemID = model.ProblemId,
                CreationDate = DateTime.Now,
                Description = model.Response,
                Status = 1,
            };
            uow.StudentProblemBids.Insert(problem);
            uow.Save();
            return RedirectToAction("problem_detail", "student", new { problem = model.ProblemId });
        }

        public ActionResult Bids(string problem)
        {
            if (problem != null)
            {
                UnitOfWork uow = new UnitOfWork();
                List<BidsModel> model = uow.UserRepository.GetBidsByProblemId(problem);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public new ActionResult Profile()
        {
            UserService userService = new UserService();
            User user = new User();
            var userId = Session["UserId"];
            user = userService.GetProfile(userId.ToString());
            if (user != null)
            {
                ProfileModel profile = new ProfileModel
                {
                    UserId = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = Encryptor.Decrypt(user.Email),
                    //Picture = user.Picture,
                    Address = user.Address,
                    Country = user.Country,
                    City = user.City,
                    //ProfileImage = user.Picture,
                    //BirthDate = (DateTime)user.BirthDate,
                    ContactNumber = user.ContactNumber

                };
                return View(profile);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Filters.AuthorizationFilter]
        public new ActionResult Profile(ProfileModel profile, HttpPostedFileBase file)
        {
            UserService userService = new UserService();
            var userId = Session["UserId"].ToString();
            User user = userService.GetProfile(userId);
            if (file != null)
            {
                profile.ProfileImage = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), profile.ProfileImage);
                file.SaveAs(path);

                //byte[] bytes;
                //using (BinaryReader br = new BinaryReader(file.InputStream))
                //{
                //    bytes = br.ReadBytes(file.ContentLength);
                //}

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
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/emailconfirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
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
            //List<StudentClasses> sc = uow.StudentRepository.GetClasses(Session["UserId"].ToString(), (int)ClassJoinStatus.Approved);
            List<StudentClasses> sc = uow.StudentRepository.GetClasses(Session["UserId"].ToString());
            StudentClassesViewModel model = new StudentClassesViewModel();
            //model.Pending = sc.Where(e => e.ClassDate > DateTime.Now && (e.RequestStatus == (int)ClassJoinStatus.Pending || e.RequestStatus == (int)ClassJoinStatus.Rejected)).ToList();
            model.Pending = sc.Where(e => (e.RequestStatus == (int)ClassJoinStatus.Pending || e.RequestStatus == (int)ClassJoinStatus.Rejected)).ToList();
            model.Upcoming = sc.Where(e => e.ClassDate > DateTime.Now && e.RequestStatus == (int)ClassStatus.Approved).ToList();
            model.Past = sc.Where(e => e.ClassDate < DateTime.Now && e.RequestStatus == (int)ClassStatus.Approved).ToList();
            model.Cancelled = sc.Where(e => e.ClassStatus == (int)ClassStatus.Cancelled).ToList();
            //model.Recorded = sc.Where(e => e.ClassDate < DateTime.Now && e.ClassStatus != (int)ClassStatus.Cancelled && e.Record == true).ToList();
            return View(model);
        }

        public ActionResult Wallet()
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                string userid = Session["UserId"].ToString();
                var stuCredits = uow.StudentCredits.Get().Where(a => a.StudentID == userid).FirstOrDefault();
                if (stuCredits != null)
                {
                    ViewBag.TotalBalance = stuCredits.TotalCredits;
                }
                else
                {
                    ViewBag.TotalBalance = 0;
                }

                ViewBag.Subscriptions = uow.Subscriptions.Get().Where(a => a.UserID == userid).ToList();
                ViewBag.CreditLog = uow.StudentCreditLogs.Get().Where(a => a.StudentID == userid).ToList();
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ActionResult Notifications()
        {
            string userId = Session["UserId"].ToString();
            UnitOfWork uow = new UnitOfWork();
            List<Notification> notifications = uow.Notifications.Get(x => x.ToUser == userId).OrderByDescending(x => x.CreationDate).ThenByDescending(x => x.IsRead).ToList();
            return View(notifications);
        }

        public ActionResult Teachers(string q, int s = 0)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.SearchTeachers(q, s);
            return View(model);
        }
    }
}