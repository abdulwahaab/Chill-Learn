using System;
using System.Linq;
using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using ChillLearn.Data.Models;
using System.Collections.Generic;

namespace ChillLearn.Controllers
{
    [Filters.AdminAuth]
    public class AdminController : BaseController
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

        //manage plans
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
                    Hours = model.Credits
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

        public ActionResult Plan(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            Plan plan = uow.Plans.Get().Where(a => a.PlanID == id).FirstOrDefault();
            return View(plan);
        }

        [HttpPost]
        public ActionResult Plan(Plan plan, string update, string disable, string enable)
        {
            UnitOfWork uow = new UnitOfWork();
            Plan plan1 = uow.Plans.Get().Where(a => a.PlanID == plan.PlanID).FirstOrDefault();
            if (!string.IsNullOrEmpty(update))
            {
                if (plan1 != null)
                {
                    plan1.PlanName = plan.PlanName;
                    plan1.Price = plan.Price;
                    plan1.Hours = plan.Hours;
                    uow.Plans.Update(plan1);
                }

            }
            //if (!string.IsNullOrEmpty(delete))
            //{
            //    if (plan1 != null)
            //    {
            //        uow.Plans.Delete(plan1);
            //    }
            //}
            if (!string.IsNullOrEmpty(disable))
            {
                if (plan1 != null)
                {
                    plan1.Status = 0;
                    uow.Plans.Update(plan1);
                }
            }
            if (!string.IsNullOrEmpty(enable))
            {
                if (plan1 != null)
                {
                    plan1.Status = 1;
                    uow.Plans.Update(plan1);
                }
            }
            uow.Save();
            return RedirectToAction("plans", "admin");
        }

        //Manage  Tutors
        public ActionResult ManageTutors()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Teacher && a.Status == (int)UserStatus.Approved).ToList();
            return View(users);
        }

        public ActionResult TutorRequests()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Teacher && a.Status == (int)UserStatus.Verified).ToList();
            return View(users);
        }

        public ActionResult TutorBlocked()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Teacher && a.Status == (int)UserStatus.Blocked).ToList();
            return View(users);
        }

        //Manage Students
        public ActionResult ManageStudents()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Student && (a.Status == (int)UserStatus.Approved || a.Status == (int)UserStatus.Verified)).ToList();
            return View(users);
        }

        public ActionResult StudentBlocked()
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> users = uow.Users.Get().Where(a => a.UserRole == (int)UserRoles.Student && a.Status == (int)UserStatus.Blocked).ToList();
            return View(users);
        }

        //public ActionResult Request(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        UnitOfWork uow = new UnitOfWork();
        //        RequestViewModel viewModel = new RequestViewModel();
        //        viewModel.User = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();
        //        viewModel.User.Email = Encryptor.Decrypt(viewModel.User.Email);
        //        viewModel.TeacherDetail = uow.TeacherDetails.Get().Where(a => a.TeacherID == id).FirstOrDefault();
        //        viewModel.TeacherFiles = uow.TeacherFiles.Get().Where(a => a.TeacherID == id).ToList();
        //        viewModel.TeacherSubjects = uow.TeacherStages.Get().Where(a => a.TeacherID == id).ToList();

        //        return View(viewModel);
        //    }
        //    else
        //    {
        //        return RedirectToAction("tutorrequests");
        //    }
        //}
        //[HttpPost]
        //public ActionResult Request(RequestParam model)
        //{
        //    string id = model.UserId;
        //    UnitOfWork uow = new UnitOfWork();
        //    var user = uow.Users.Get().Where(a => a.UserID == model.UserId).FirstOrDefault();
        //    if(user != null)
        //    {
        //        if (model.Status == "Accept")
        //        {
        //            user.Status = (int)UserStatus.Approved;
        //            ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;
        //        }
        //        else
        //        {
        //            user.Status = (int)UserStatus.Deleted;
        //            ViewBag.Message = Resources.Resources.MsgTeacherDeclineSuccess;
        //        }
        //        uow.Users.Update(user);
        //        uow.Save();
        //    }
        //    RequestViewModel viewModel = new RequestViewModel();
        //    viewModel.User = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();
        //    viewModel.User.Email = Encryptor.Decrypt(viewModel.User.Email);
        //    viewModel.TeacherDetail = uow.TeacherDetails.Get().Where(a => a.TeacherID == id).FirstOrDefault();
        //    viewModel.TeacherFiles = uow.TeacherFiles.Get().Where(a => a.TeacherID == id).ToList();
        //    viewModel.TeacherSubjects = uow.TeacherStages.Get().Where(a => a.TeacherID == id).ToList();

        //    return View(viewModel);
        //}

        public ActionResult Detail(string id)
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
        public ActionResult detail(RequestParam model)
        {
            string id = model.UserId;
            UnitOfWork uow = new UnitOfWork();
            var user = uow.Users.Get().Where(a => a.UserID == model.UserId).FirstOrDefault();
            if (user != null)
            {
                string status = "";
                if (model.Status == "block")
                {
                    user.Status = (int)UserStatus.Blocked;
                    ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;
                }
                else if (model.Status == "active")
                {
                    status = "approved";
                    user.Status = (int)UserStatus.Approved;
                    ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;
                }
                else if (model.Status == "delete")
                {
                    status = "rejected";
                    user.Status = (int)UserStatus.Deleted;
                    ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;

                }
                uow.Users.Update(user);
                uow.Save();
                //add notification
                Common.AddNotification("You profile has been " + status, "", "admin", model.UserId, "", (int)NotificationType.Teacher);
            }
            RequestViewModel viewModel = new RequestViewModel();
            viewModel.User = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();
            viewModel.User.Email = Encryptor.Decrypt(viewModel.User.Email);
            viewModel.TeacherDetail = uow.TeacherDetails.Get().Where(a => a.TeacherID == id).FirstOrDefault();
            viewModel.TeacherFiles = uow.TeacherFiles.Get().Where(a => a.TeacherID == id).ToList();
            viewModel.TeacherSubjects = uow.TeacherStages.Get().Where(a => a.TeacherID == id).ToList();

            return View(viewModel);
        }

        //student detail 
        public ActionResult Student(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                UnitOfWork uow = new UnitOfWork();
                User user = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();
                user.Email = Encryptor.Decrypt(user.Email);

                return View(user);
            }
            else
            {
                return RedirectToAction("managestudents");
            }
        }

        [HttpPost]
        public ActionResult Student(RequestParam model)
        {
            string id = model.UserId;
            UnitOfWork uow = new UnitOfWork();
            var user = uow.Users.Get().Where(a => a.UserID == model.UserId && a.UserRole == (int)UserRoles.Student).FirstOrDefault();
            if (user != null)
            {
                if (model.Status == "block")
                {
                    user.Status = (int)UserStatus.Blocked;
                    ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;
                }
                else if (model.Status == "active")
                {
                    user.Status = (int)UserStatus.Approved;
                    ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;
                }
                else if (model.Status == "delete")
                {
                    user.Status = (int)UserStatus.Deleted;
                    ViewBag.Message = Resources.Resources.MsgTeacherApproveSuccess;

                }
                uow.Users.Update(user);
                uow.Save();
            }
            user.Email = Encryptor.Decrypt(user.Email);
            //User user = uow.Users.Get().Where(a => a.UserID == id).FirstOrDefault();

            return View(user);
        }
    }
}