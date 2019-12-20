using System;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using ChillLearn.Helpers;
using ChillLearn.DAL;
using System.Linq;
using System.Web.Routing;

namespace ChillLearn.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            GetUserNotifications();
        }

        public ActionResult Arabic()
        {
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = "ar-SA";
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = "ar-SA";
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.PathAndQuery);
            //return RedirectToAction("Index", "Home");
        }

        public ActionResult English()
        {
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = "en-US";
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = "en-US";
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.PathAndQuery);
            //return RedirectToAction("Index", "Home");
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }

        public void GetUserNotifications()
        {
            if (Session != null && Session["UserId"] != null)
            {
                string userId = Session["UserId"].ToString();
                UnitOfWork uow = new UnitOfWork();
                ViewBag.Notifications = uow.Notifications.Get(x => x.ToUser == userId).OrderByDescending(x => x.CreationDate).ToList();
            }
        }

        public ActionResult Notification(int id)
        {
            Common common = new Common();
            string redirectUrl = common.MarkNotificationRead(id);
            if (!string.IsNullOrEmpty(redirectUrl))
                return Redirect(redirectUrl);
            else
                return Redirect(Request.UrlReferrer.LocalPath);
        }

        //[HttpPost]
        //public ActionResult ChangeLanguage(int language)
        //{
        //    if (language == 1)
        //        return RedirectToAction("English");
        //    else
        //        return RedirectToAction("Arabic");
        //}
    }
}