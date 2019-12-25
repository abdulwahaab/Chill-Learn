using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;


namespace ChillLearn.Controllers
{
    [Filters.AuthorizationFilter]
    public class BidController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string id, string accept, string decline)
        {
            if (id != null)
            {
                if (!string.IsNullOrEmpty(accept))
                    UpdateClassStatus(id, "accept");
                else if (!string.IsNullOrEmpty(decline))
                    UpdateClassStatus(id, "decline");

                UnitOfWork uow = new UnitOfWork();
                BidDetailModel model = new BidDetailModel();
                string userId = Session["UserId"].ToString();
                StudentProblemDetailModel probDetail = uow.UserRepository.GetProblemDetailByBidId(id, userId);
                if (probDetail != null)
                {
                    model.ProblemDetail = probDetail;
                    model.Messages = uow.UserRepository.GetMessagesByBidId(id);
                    model.BidId = id;
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
            Common.AddNotification(Session["UserName"].ToString() + " sent you a message", "", fromUser, toUser, "/problem/proposal/" + model.BidId, (int)NotificationType.Message);
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

        public void UpdateClassStatus(string bidId, string status)
        {
            try
            {
                string message = "";
                string userId = Session["UserId"].ToString();
                string userName = Session["UserName"].ToString();
                UnitOfWork uow = new UnitOfWork();
                StudentProblemBid bid = uow.StudentProblemBids.GetByID(bidId);
                StudentProblem problem = uow.StudentProblems.GetByID(bid.ProblemID);
                Class classDetail = uow.Classes.Get(x => x.ProblemID == problem.ProblemID).FirstOrDefault();
                if (status == "accept")
                {
                    if (Common.UserHasCredits(problem.StudentID, (decimal)problem.HoursNeeded))
                    {
                        bid.Status = (int)BidStatus.Accepted;
                        classDetail.Status = (int)ClassStatus.OfferAccepted;
                        Common.AddNotification("Your offer is accepted by " + userName, "", userId, problem.StudentID, "/problem/proposal/" + bid.BidID, (int)NotificationType.Class);
                        message = "Your offer is accepted by ";
                        int braincertClassId = 0;
                        if (classDetail.Type == (int)SessionType.Live)
                        {
                            braincertClassId = Convert.ToInt32(CreateBrainCertClass(classDetail.Title, classDetail.ClassDate.ToString("MM/dd/yyyy HH:mm"), classDetail.StartTime, classDetail.EndTime, Convert.ToInt32(classDetail.Record)));
                            classDetail.BrainCertId = braincertClassId;
                        }
                        DeductStudentCredits(problem.StudentID, classDetail.ClassID, (decimal)classDetail.Duration);
                    }
                    else
                        ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
                }
                else if (status == "decline")
                {
                    bid.Status = (int)BidStatus.Declined;
                    classDetail.Status = (int)ClassStatus.OfferDeclined;
                    Common.AddNotification("Your offer is declined by " + userName, "", userId, problem.StudentID, "/problem/proposal/" + bid.BidID, (int)NotificationType.Class);
                    message = "Your offer is declined by ";
                }
                Message msg = new Message
                {
                    BidID = bid.BidID,
                    FromUser = userId,
                    ToUser = bid.UserID,
                    CreationDate = DateTime.Now,
                    Message1 = message + userName,
                    Status = 1,
                };
                uow.Messages.Insert(msg);
                uow.Save();
                uow.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public void DeductStudentCredits(string studentId, string classId, decimal hours)
        {
            UnitOfWork uow = new UnitOfWork();
            StudentCredit studentCredit = uow.StudentCredits.Get(a => a.StudentID == studentId).FirstOrDefault();
            if (studentCredit != null && studentCredit.TotalCredits >= hours)
            {
                string userId = Session["UserId"].ToString();
                string className = uow.Classes.Get(x => x.ClassID == classId).FirstOrDefault().Title;
                studentCredit.TotalCredits = studentCredit.TotalCredits - hours;
                studentCredit.UsedCredits = studentCredit.UsedCredits + hours;
                uow.StudentCredits.Update(studentCredit);
                StudentCreditLog studentCreditLog = new StudentCreditLog
                {
                    ClassID = classId,
                    CreationDate = DateTime.Now,
                    CreditsUsed = hours,
                    UserID = studentId,
                    LogType = "Deducted"
                };
                uow.StudentCreditLogs.Insert(studentCreditLog);
                uow.Save();
                uow.Dispose();
                //Common.AddNotification(Session["UserName"].ToString() + " accepted your offer", "",
                //        userId, studentId, "/student/classes", (int)NotificationType.Class);
                //payment deduction notification
                Common.AddNotification("Your wallet has been deducted with " + hours + " hours for class " + className, "",
                    userId, studentId, "/student/wallet", (int)NotificationType.Wallet);
            }
        }

        public string CreateBrainCertClass(string title, string date, string startTime, string endTime, int record)
        {
            string[] dateArray = date.Split(' ')[0].Split('/');
            string properDate = dateArray[2] + "-" + dateArray[0] + "-" + dateArray[1];
            BrainCert bc = new BrainCert();
            BrainCertClass bClass = new BrainCertClass();
            bClass.Title = title;
            bClass.Date = properDate;
            bClass.StartTime = startTime;
            bClass.EndTime = endTime;
            bClass.Record = record;
            return bc.CreateClassAsync(bClass).ToString();
        }
    }
}