using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.App_Start
{
    public class AuthorizationFilter : ActionFilterAttribute, IActionFilter  
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
            if (HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserName"].ToString() != "Mirza")
            { 
               filterContext.Result = new RedirectResult("~/account/login");  
           }  
       }
    } 
}