using ChillLearn.Enums;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Filters
{
    public class ApprovedFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            // Check for authorization  
            if (HttpContext.Current.Session["UserRole"] == null || (HttpContext.Current.Session["UserRole"] != null && (int)HttpContext.Current.Session["UserStatus"] != (int)UserStatus.Approved))
            {
                filterContext.Result = new RedirectResult("~/account/login");
            }
        }
    }
}