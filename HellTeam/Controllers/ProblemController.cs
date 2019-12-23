using System;
using System.IO;
using System.Web;
using System.Linq;
using ChillLearn.DAL;
using System.Web.Mvc;
using ChillLearn.Enums;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using ChillLearn.ViewModels;
using System.Globalization;

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

        public ActionResult Proposal(string id, string accept, string decline)
        {
            if (!string.IsNullOrEmpty(accept))
                UpdateProposalStatus(id, "accept");
            else if (!string.IsNullOrEmpty(decline))
                UpdateProposalStatus(id, "decline");

            if (id != null)
            {
                UnitOfWork uow = new UnitOfWork();
                ProposalDetailModel model = new ProposalDetailModel();
                string userId = Session["UserId"].ToString();
                StudentProblemDetailModel probDetail = uow.UserRepository.GetProblemDetailByBidId(id, userId);
                if (probDetail != null)
                {
                    model.ProblemDetail = probDetail;
                    model.Messages = uow.UserRepository.GetMessagesByBidId(id);
                    model.BidId = id;

                    //display class data
                    List<Subject> subjects = uow.Subjects.Get().ToList();
                    model.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
                    model.SessionTypes = GetSessionTypes();

                }
                else
                {
                    ViewBag.Msg = "No Data Is Available";
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("student", "problems");
            }
        }

        [HttpPost]
        public ActionResult BidResponse(BidDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("detail", "bid", new
                {
                    id = model.BidId
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
            Common.AddNotification(Session["UserName"].ToString() + " sent you a message", "", fromUser, toUser, "/bid/detail/" + model.BidId,
                (int)NotificationType.Message);
            return RedirectToAction("detail", "bid", new
            {
                id = model.BidId
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

        public void UpdateProposalStatus(string bidId, string status)
        {
            UnitOfWork uow = new UnitOfWork();
            StudentProblemBid bid = uow.StudentProblemBids.GetByID(bidId);

            if (status == "viewed")
                bid.Status = (int)BidStatus.Viewed;
            else if (status == "accept")
            {
                StudentProblem problem = uow.StudentProblems.GetByID(bid.ProblemID);
                if (Common.UserHasCredits(problem.StudentID, (decimal)problem.HoursNeeded))
                {
                    bid.Status = (int)BidStatus.Accepted;
                    problem.Status = (int)ProblemStatus.BidAccepted;
                    Common.AddNotification("Your proposal has been accepted by " + Session["UserName"].ToString(), "", Session["UserId"].ToString(), bid.UserID, "/bid/detail/" + bid.BidID, (int)NotificationType.Bid);
                }
                else
                    ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
            }
            else if (status == "decline")
            {
                bid.Status = (int)BidStatus.Declined;
                Common.AddNotification("Your proposal has been declined.", "", Session["UserId"].ToString(), bid.UserID, "/bid/detail/" + bid.BidID, (int)NotificationType.Bid);
            }
            uow.Save();
            uow.Dispose();
        }

        public List<SelectListItem> GetSessionTypes()
        {
            List<SelectListItem> sessionTypes = Enum.GetValues(typeof(SessionType))
            .Cast<SessionType>()
            .Select(t => new SelectListItem
            {
                Value = Convert.ToInt16(t).ToString(),
                Text = Enumerations.GetEnumDescription(t)
            }).ToList();

            return sessionTypes;
        }

        [HttpPost]
        public ActionResult Class(ProposalDetailModel model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                if (!ModelState.IsValid)
                {
                    model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                    model.SessionTypes = GetSessionTypes();
                    return View(model);
                }
                else
                {
                    //CreateBrainCertClass(model.Title, model.Date, model.StartTime, model.Duration.ToString(), model.Record);
                    bool record = false;
                    if (model.Record == "1")
                    {
                        record = true;
                    }
                    UpdateProposalStatus(model.BidId, "accept");
                    string date = model.Date + " " + model.StartTime.Insert(model.StartTime.Length - 2, " ");
                    DateTime combDT = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    Class clsCreate = new Class()
                    {
                        ClassID = Guid.NewGuid().ToString(),
                        Title = model.Title,
                        ClassDate = combDT,
                        ClassTime = model.StartTime,
                        Duration = model.Duration,
                        CreationDate = DateTime.Now,
                        Type = model.SessionType,
                        Record = record,
                        CreatedBy = Session["UserId"].ToString(),
                        TeacherID = model.ProblemDetail.TeacherID,
                        Description = model.Description,
                        SubjectID = model.Subject,
                        Status = (int)ClassStatus.Pending,
                        BrainCertId = model.BrainCertId
                    };
                    uow.Classes.Insert(clsCreate);
                    uow.Save();

                    ClassViewModel classView = new ClassViewModel();
                    classView.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                    classView.SessionTypes = GetSessionTypes();
                    ModelState.AddModelError("success", Resources.Resources.MsgClassCreatedSuccess);
                    return View(classView);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //public string CreateBrainCertClass(string title, string date, string startTime, string endTime, string isRecord)
        //{
        //    string[] dateArray = date.Split('/');
        //    string properDate = dateArray[2] + "-" + dateArray[1] + "-" + dateArray[0];
        //    BrainCert bc = new BrainCert();
        //    BrainCertClass bClass = new BrainCertClass();
        //    bClass.Title = title;
        //    bClass.Date = properDate;
        //    bClass.StartTime = startTime;
        //    bClass.EndTime = endTime;
        //    bClass.IsRecord = isRecord;
        //    return bc.CreateClassAsync(bClass).ToString();
        //}

        //public string CreateBrainCertClass(ClassViewModel classModel)
        //{
        //    BrainCert bc = new BrainCert();
        //    BrainCertClass bClass = new BrainCertClass();
        //    bClass.Title = classModel.Title;
        //    bClass.Date = classModel.Date;
        //    bClass.StartTime = classModel.Time;
        //    bClass.EndTime = classModel.Duration.ToString();
        //    bClass.IsRecord = classModel.Record;
        //    return bc.CreateClassAsync(bClass).ToString();
        //}
    }
}