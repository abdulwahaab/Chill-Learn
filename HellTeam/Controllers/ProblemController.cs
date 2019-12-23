using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.CustomModels;
using System.Collections.Generic;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizationFilter]
    public class ProblemController : BaseController
    {
        public ActionResult Bids(string id)
        {
            if (id != null)
            {
                UnitOfWork uow = new UnitOfWork();
                List<BidsModel> model = uow.UserRepository.GetBidsByProblemId(id);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}