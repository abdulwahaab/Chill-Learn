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
        public ActionResult Tutor_Requests()
        {
            return View();
        }
        public ActionResult Tutor_Application()
        {
            return View();
        }
    }
}