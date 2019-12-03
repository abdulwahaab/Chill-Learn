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
    }
}