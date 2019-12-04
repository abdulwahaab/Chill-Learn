using ChillLearn.DAL;
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
    [Filters.AdminAuth]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Manage_Admin()
        {
            return View();
        }
        public ActionResult Registered_Member()
        {
            return View();
        }
        public ActionResult Notification()
        {
            return View();
        }
        public ActionResult Tutor_Application()
        {
            return View();
        }
        public ActionResult Plans()
        {
            UnitOfWork uow = new UnitOfWork();
            List<Plan> plans = uow.Plans.Get().ToList();
            return View(plans);
        }

        [HttpPost]
        public string CreatePlans(PlanParam model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return "ve";
                //}
                UnitOfWork uow = new UnitOfWork();
                Plan plan = new Plan()
                {
                    PlanID = Guid.NewGuid().ToString(),
                    PlanName = model.PlanName,
                    Price = model.Price,
                    Status = 1,
                    Credits = model.Credits
                };
                uow.Plans.Insert(plan);
                uow.Save();
                return "success";
            }
            catch (Exception ex)
            {
                return "ex";
            }

        }

        public ActionResult ManageTutors()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Teacher && a.Status == (int)UserStatus.Approved).ToList();
            return View(users);
        }
        public ActionResult TutorRequests()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Teacher &&  a.Status == (int)UserStatus.Verified).ToList();
            return View(users);
        }
        
        public ActionResult Request(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                UnitOfWork uow = new UnitOfWork();
                RequestViewModel viewModel = new RequestViewModel();
                viewModel.User = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();
                viewModel.User.Email = Encryptor.Decrypt(viewModel.User.Email);
                viewModel.TeacherDetail = uow.TeacherDetails.Get().Where(a => a.TeacherID == id).FirstOrDefault();
                viewModel.TeacherFiles = uow.TeacherFiles.Get().Where(a => a.TeacherID == id).ToList();
                viewModel.TeacherSubjects = uow.TeacherStages.Get().Where(a => a.TeacherID == id).ToList();

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("tutorrequests");
            }
        }
        [HttpPost]
        public ActionResult Request(RequestParam model)
        {
            string id = model.UserId;
            UnitOfWork uow = new UnitOfWork();
            var user = uow.Users.Get().Where(a => a.UserID == model.UserId).FirstOrDefault();
            if(user != null)
            {
                if (model.Status == "Accept")
                {
                    user.Status = (int)UserStatus.Approved;
                    ViewBag.Message = "User Request Approved Successfully";
                }
                else
                {
                    user.Status = (int)UserStatus.Deleted;
                    ViewBag.Message = "User Request Decilne Successfully";
                }
                uow.Users.Update(user);
                uow.Save();
            }
            RequestViewModel viewModel = new RequestViewModel();
            viewModel.User = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();
            viewModel.User.Email = Encryptor.Decrypt(viewModel.User.Email);
            viewModel.TeacherDetail = uow.TeacherDetails.Get().Where(a => a.TeacherID == id).FirstOrDefault();
            viewModel.TeacherFiles = uow.TeacherFiles.Get().Where(a => a.TeacherID == id).ToList();
            viewModel.TeacherSubjects = uow.TeacherStages.Get().Where(a => a.TeacherID == id).ToList();

            return View(viewModel);
        }
        
    }
}