using System.Web.Mvc;

// to work with Resources file you should add these library
//using System.Windows.Forms;
//using System.Resources;
//using System.Configuration;
//using System.IO;
//using System.Reflection;
//using System.Globalization;

namespace ChillLearn.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "account");
            }
         
        }




    }
}