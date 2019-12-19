using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizationFilter]
    public class BidController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(string b)
        {
            if (b != null)
            {
                UnitOfWork uow = new UnitOfWork();
                BidDetailModel model = new BidDetailModel();
                string userId = Session["UserId"].ToString();
                StudentProblemDetailModel probDetail = uow.UserRepository.GetProblemDetailByBidId(b, userId);
                if (probDetail != null)
                {
                    model.ProblemDetail = probDetail;
                    model.Messages = uow.UserRepository.GetMessagesByBidId(b);
                    model.BidId = b;
                }
                else
                {
                    ViewBag.Msg = "No Data Is Available";
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult BidResponse(BidDetailModel model)
        {
            UnitOfWork uow = new UnitOfWork();
            if (!ModelState.IsValid)
            {
                string userId = Session["UserId"].ToString();
                model.ProblemDetail = uow.UserRepository.GetProblemDetailByBidId(model.BidId, userId);
                model.Messages = uow.UserRepository.GetMessagesByBidId(model.BidId);
                return View("detail", model);
            }
            Message msg = new Message
            {
                BidID = model.BidId,
                FromUser = Session["UserId"].ToString(),
                ToUser = model.ToUser,
                CreationDate = DateTime.Now,
                Message1 = model.Response,
                Status = 1,
            };
            uow.Messages.Insert(msg);
            uow.Save();
            return RedirectToAction("detail","bid", new
            {
                b = model.BidId
            });
        }

  
    }
}