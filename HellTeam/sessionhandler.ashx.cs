using System;
using System.Web;
using System.Web.SessionState;

namespace ChillLearn
{
    public class sessionhandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Session["KeepAliveFrontEnd"] = DateTime.Now;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}