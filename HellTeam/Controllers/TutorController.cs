using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeTeacher]
    public class TutorController : Controller
    {
        // GET: Tutor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Applications()
        {
            return View();
        }

        public ActionResult Payment_Roll()
        {
            return View();
        }
        public ActionResult Payment_History()
        {
            return View();
        }
        public ActionResult Tutor_Profile()
        {
            return View();
        }

        public ActionResult Search_Question()
        {
            UnitOfWork uow = new UnitOfWork();
            
            List<StudentProblemsModel> problems = uow.UserRepository.GetProblems(Session["UserId"].ToString());
            return View(problems);
        }
        public ActionResult question_detail(string q)
        {
            if (q != null)
            {
                UnitOfWork uow = new UnitOfWork();
                QuestionDetailModel model = new QuestionDetailModel();
                model.QuestionDetail = uow.UserRepository.GetQuestionDetailById(q);
                if (model.QuestionDetail != null)
                {
                    model.ProblemId = q;
                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult PostBid(QuestionDetailModel model)
        {
            UnitOfWork uow = new UnitOfWork();

            if (!ModelState.IsValid)
            {
                model.QuestionDetail = uow.UserRepository.GetQuestionDetailById(model.ProblemId);
                //return View(model);
                return View("question_detail", model);
            }
            StudentProblemBid problem = new StudentProblemBid
            {
                BidID = Guid.NewGuid().ToString(),
                UserID = Session["UserId"].ToString(),
                ProblemID = model.ProblemId,
                CreationDate = DateTime.Now,
                Description = model.Response,
                Status = (int)BidStatus.Pending,
            };
            uow.StudentProblemBids.Insert(problem);
            uow.Save();
            return RedirectToAction("search_question", "tutor");
        }
    }
}