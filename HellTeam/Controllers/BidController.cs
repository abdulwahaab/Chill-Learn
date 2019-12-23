using System;
using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using ChillLearn.Enums;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        [HttpPost]
        public ActionResult BidResponse(BidDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("detail", "bid", new
                {
                    b = model.BidId
                });
            }
            string userId = Session["UserId"].ToString();
            UnitOfWork uow = new UnitOfWork();
            model.ProblemDetail = uow.UserRepository.GetProblemDetailByBidId(model.BidId, userId);
            model.Messages = uow.UserRepository.GetMessagesByBidId(model.BidId);

            string toUser = "", fromUser = userId;
            if (userId == model.ProblemDetail.StudentID)
            {
                toUser = model.ProblemDetail.TeacherID;
                fromUser = model.ProblemDetail.StudentID;
            }
            else
            {
                toUser = model.ProblemDetail.StudentID;
                fromUser = model.ProblemDetail.TeacherID;
            }
            string fileHTML = SaveFiles(model.Files, userId, model.ProblemDetail.ProblemID);
            Message msg = new Message
            {
                BidID = model.BidId,
                FromUser = fromUser,
                ToUser = toUser,
                CreationDate = DateTime.Now,
                Message1 = model.Response + fileHTML,
                Status = 1,
            };
            uow.Messages.Insert(msg);
            uow.Save();
            //send messge notification
            Common.AddNotification(Session["UserName"].ToString() + " sent you a message", "", fromUser, toUser, "/bid/detail?b=" + model.BidId, (int)NotificationType.Message);
            return RedirectToAction("detail", "bid", new
            {
                b = model.BidId
            });
        }

        public string SaveFiles(List<HttpPostedFileBase> files, string userId, string problemId)
        {
            UnitOfWork uow = new UnitOfWork();
            string fileHTML = "";
            try
            {
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string fileName = null;
                        fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads/QuestionFiles/"), fileName);
                        if (System.IO.File.Exists(path))
                        {
                            string[] fileNameSplit = fileName.Split('.');
                            if (fileNameSplit.Count() > 1)
                                fileName = fileName.Split('.')[0] + "_2." + fileName.Split('.')[1];
                            else
                                fileName = fileName + "_2";
                            path = Path.Combine(Server.MapPath("~/Uploads/QuestionFiles/"), fileName);
                        }
                        file.SaveAs(path);
                        StudentProblemFile problemFile = new StudentProblemFile
                        {
                            ProblemID = problemId,
                            FileName = fileName,
                            CreationDate = DateTime.Now,
                            UserID = userId
                        };
                        uow.StudentProblemFiles.Insert(problemFile);
                        fileHTML += "<br/><a target='_blank' href='/uploads/questionfiles/" + fileName + "'>" + fileName + "</a>";
                    }
                    uow.Save();
                }
            }
            catch (Exception)
            {
            }
            return fileHTML;
        }
    }
}