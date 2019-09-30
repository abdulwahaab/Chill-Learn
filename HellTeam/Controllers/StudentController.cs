using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.DAL.Services;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeStudent]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Refund_Request()
        {
            return View();
        }
        public List<SelectListItem> GetSessionTypess()
        {
            List<SelectListItem> userRoles = Enum.GetValues(typeof(SessionType))
                                              .Cast<SessionType>()
                                              .Select(t => new SelectListItem
                                              {
                                                  Value = Convert.ToInt16(t).ToString(),
                                                  Text = Enumerations.GetEnumDescription(t)
                                              }).ToList();

            return userRoles;
        }
        public ActionResult Problems()
        {
            UnitOfWork uow = new UnitOfWork();
            ProblemsModel model = new ProblemsModel();
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            model.SessionTypes = GetSessionTypess();
            return View(model);
        }
        [HttpPost]
        public ActionResult Problems(ProblemsModel model, HttpPostedFileBase file)
        {
            UnitOfWork uow = new UnitOfWork();
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");     
            model.SessionTypes = GetSessionTypess();
            if (!ModelState.IsValid)
            {
                model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
                ModelState.AddModelError("error", "Please provide valid information.");
                return View(model);
            }
            string fileName = "";
            if (file != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                file.SaveAs(path);
            }
            StudentProblem problem = new StudentProblem
            {
                ProblemID = Guid.NewGuid().ToString(),
                StudentID = Session["UserId"].ToString(),
                SubjectID = model.Subject,
                CreationDate = DateTime.Now,
                Description = model.ProblemDescription,
                HoursNeeded = model.HoursNeeded,
                Type = model.Type,
                FileName = fileName,
                ExpireDate = model.DeadLine // need to add datetime datepicker
            };
            uow.StudentProblems.Insert(problem);
            uow.Save();
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            ModelState.AddModelError("success", "Problem submited successfully");
            return View(model);
        }
        public ActionResult Problem_Detail(string problem)
        {
            if(problem != null)
            {
                UnitOfWork uow = new UnitOfWork();
                ProblemDetailModel model = new ProblemDetailModel();
                model.ProblemDetails = uow.UserRepository.GetQuestionDetailById(problem);
                if (model.ProblemDetails != null)
                {
                    model.ProblemId = problem;
                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Problem_Detail(ProblemDetailModel model)
        {
            UnitOfWork uow = new UnitOfWork();

            if (!ModelState.IsValid)
            {
                model.ProblemDetails = uow.UserRepository.GetQuestionDetailById(model.ProblemId);
                return View(model);
            }
            StudentProblemBid problem = new StudentProblemBid
            {
                BidID = Guid.NewGuid().ToString(),
                UserID = Session["UserId"].ToString(),
                ProblemID = model.ProblemId,
                CreationDate = DateTime.Now,
                Description = model.Response,
                Status = 1,
            };
            uow.StudentProblemBids.Insert(problem);
            uow.Save();
            return RedirectToAction("problem_detail", "student", new { problem = model.ProblemId });
        }
        public ActionResult Bids(string problem)
        {
            if (problem != null)
            {
                UnitOfWork uow = new UnitOfWork();
                List<BidsModel> model = uow.UserRepository.GetBidsByProblemId(problem);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Bid_Detail(string b)
        {
            if (b != null)
            {
                UnitOfWork uow = new UnitOfWork();
                BidDetailModel model = new BidDetailModel();
                model.ProblemDetail = uow.UserRepository.GetProblemDetailByBidId(b);
                model.Messages = uow.UserRepository.GetMessagesByBidId(b);
                model.BidId = b;
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
                model.ProblemDetail = uow.UserRepository.GetProblemDetailByBidId(model.BidId);
                model.Messages = uow.UserRepository.GetMessagesByBidId(model.BidId);
                return View("bid_detail", model);
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
            return RedirectToAction("bid_detail", new
            {
                b = model.BidId
            });
        }
    }
}