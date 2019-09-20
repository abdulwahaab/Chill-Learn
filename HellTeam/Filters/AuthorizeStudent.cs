using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChillLearn.Enums;

namespace ChillLearn.Filters
{
    public class AuthorizeStudent : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller  
                return;
            }

            // Check for authorization  
            if (HttpContext.Current.Session["UserRole"] == null || (HttpContext.Current.Session["UserRole"] != null && (int)HttpContext.Current.Session["UserRole"] != (int)UserRoles.Student))
            {
                filterContext.Result = new RedirectResult("~/account/login");
            }
        }
    }
}