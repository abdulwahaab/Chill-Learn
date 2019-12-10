using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizationFilter]
    public class ChatController : BaseController
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Inbox(string c)
        {
            ViewBag.ToUser = c;
            return View();
        }
    }
}