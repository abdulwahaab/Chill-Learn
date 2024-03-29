﻿using System;
using System.IO;
using System.Web;
using System.Linq;
using ChillLearn.DAL;
using System.Web.Mvc;
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

        [HttpGet]
        public ActionResult Proposal(string id, string accept, string decline, string create)
        {
            if (TempData["Message"] != null && !string.IsNullOrEmpty(TempData["Message"].ToString()))
                ModelState.AddModelError("success", TempData["Message"].ToString());
            else if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
            {
                ViewBag.FormInvalid = 1;
                ModelState.AddModelError("error", TempData["Error"].ToString());
            }
            ViewBag.FormInvalid = 0;
            if (!string.IsNullOrEmpty(accept))
                UpdateProposalStatus(id, "accept");
            else if (!string.IsNullOrEmpty(decline))
                UpdateProposalStatus(id, "decline");

            if (id != null)
            {
                UnitOfWork uow = new UnitOfWork();
                ProposalDetailModel model = new ProposalDetailModel();
                string userId = Session["UserId"].ToString();
                StudentProblemDetailModel problemDetail = uow.UserRepository.GetProblemDetailByBidId(id, userId);
                if (problemDetail != null)
                {
                    model.ProblemDetail = problemDetail;
                    model.Subject = problemDetail.SubjectID;
                    model.SubjectName = problemDetail.SubjectName;
                    model.SessionType = problemDetail.SessionType;
                    model.SessionTypeName = nameof(problemDetail.SessionType);
                    model.Messages = uow.UserRepository.GetMessagesByBidId(id);
                    model.BidId = id;
                    //display class data
                    Common common = new Common();
                    model.DurationHourList = new SelectList(common.GetDurationHours());
                    model.DurationMinuteList = new SelectList(common.GetMinutes());
                    model.HourList = new SelectList(common.GetHours());
                    model.MinuteList = new SelectList(common.GetMinutes());
                    model.AMPMList = new SelectList(common.GetAMPM());
                    List<TeacherSubject> subjects = uow.TeacherRepository.GetSubjects(problemDetail.TeacherID);
                    model.TimeZones = new SelectList(uow.TimeZones.Get(), "GMT", "Name", "49");
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
        public ActionResult Proposal(ProposalDetailModel model, string accept, string decline)
        {
            if (!string.IsNullOrEmpty(accept))
                UpdateProposalStatus(model.BidId, "accept");
            else if (!string.IsNullOrEmpty(decline))
                UpdateProposalStatus(model.BidId, "decline");
            try
            {
                UnitOfWork uow = new UnitOfWork();
                string userId = Session["UserId"].ToString();
                if (Convert.ToInt32(model.DurationHour) < 1 && Convert.ToInt32(model.DurationMinutes) < 30)
                {
                    ViewBag.FormInvalid = 1;
                    ModelState.AddModelError("classtime-error", Resources.Resources.MsgClassDurationError);
                    return View(CreateClassView(model, userId));
                }
                else if (!ModelState.IsValid)
                {
                    ViewBag.FormInvalid = 1;
                    CreateClassView(model, userId);
                    return View(model);
                }
                else
                {
                    //calculate duration
                    decimal classDuration = Math.Round(Convert.ToInt16(model.DurationHour) + (Convert.ToInt16(model.DurationMinutes) / 60m), 2);
                    if (Common.UserHasCredits(userId, classDuration))
                    {
                        ViewBag.FormInvalid = 0;
                        bool record = false;
                        if (model.Record == "1" && model.SessionType == 1)
                        {
                            record = true;
                        }
                        string dateAndTime = model.Date;
                        DateTime classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (Convert.ToInt32(model.DurationHour) > 0 || Convert.ToInt32(model.DurationMinutes) > 0)
                        {
                            dateAndTime = model.Date + " " + model.ClassHour + ":" + model.ClassMinute + " " + model.ClassAMPM;
                            classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        if (model.SessionType == 1)
                        {
                            string classEndTime = classDate.AddHours(Convert.ToDouble(model.DurationHour)).AddMinutes(Convert.ToDouble(model.DurationMinutes)).ToString("HH:mm tt");
                            model.StartTime = classDate.ToString("hh:mm tt");
                            // only 30 minute class is allowed in trial period
                            model.ClassEndTime = classDate.AddMinutes(30).ToString("hh:mm tt"); //classEndTime;                        
                        }
                        Class clsCreate = new Class()
                        {
                            ClassID = Guid.NewGuid().ToString(),
                            Title = model.Title,
                            ClassDate = classDate,
                            StartTime = model.StartTime,
                            EndTime = model.ClassEndTime,
                            Duration = classDuration,
                            CreationDate = DateTime.Now,
                            Type = model.SessionType,
                            Record = record,
                            CreatedBy = Session["UserId"].ToString(),
                            TeacherID = model.TeacherID,
                            Description = model.Description,
                            SubjectID = model.Subject,
                            ProblemID = model.ProblemID,
                            Status = (int)ClassStatus.BidAccepted,
                            CreatedByStudent = true,
                            TimeZone = model.TimeZone,
                            BrainCertId = 0
                        };
                        uow.Classes.Insert(clsCreate);
                        uow.Save();
                        UpdateProposalStatus(model.BidId, "accept");
                        DeclineAllOtherProposals(model.ProblemID, model.BidId);
                        model.ProblemDetail = uow.UserRepository.GetProblemDetailByBidId(model.BidId, userId);
                        ModelState.AddModelError("success", Resources.Resources.MsgClassRequested);
                        return View(model);
                    }
                    else
                    {
                        ViewBag.FormInvalid = 1;
                        CreateClassView(model, userId);
                        ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
                        return View(model);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Error"] = Resources.Resources.MsgErrorTryAgain;
                return RedirectToAction("proposal", "problem", new
                {
                    id = model.BidId
                });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BidResponse(BidDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("proposal", "problem", new { id = model.BidId });
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
            return RedirectToAction("proposal", "problem", new { id = model.BidId });
        }

        public ProposalDetailModel CreateClassView(ProposalDetailModel model, string userId)
        {
            UnitOfWork uow = new UnitOfWork();
            Common common = new Common();
            model.DurationHourList = new SelectList(common.GetDurationHours());
            model.DurationMinuteList = new SelectList(common.GetMinutes());
            model.HourList = new SelectList(common.GetHours());
            model.MinuteList = new SelectList(common.GetMinutes());
            model.AMPMList = new SelectList(common.GetAMPM());
            model.SessionTypes = GetSessionTypes();
            model.Messages = uow.UserRepository.GetMessagesByBidId(model.BidId);
            List<TeacherSubject> subjects = uow.TeacherRepository.GetSubjects(model.TeacherID);
            model.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
            model.TimeZones = new SelectList(uow.TimeZones.Get(), "GMT", "Name", "49");
            model.ProblemDetail = uow.UserRepository.GetProblemDetailByBidId(model.BidId, userId);
            model.SubjectName = model.ProblemDetail.SubjectName;
            return model;
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
                    bid.Status = (int)BidStatus.Offered;
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

        //[HttpPost]
        //public ActionResult Class(ProposalDetailModel model)
        //{
        //    try
        //    {
        //        UnitOfWork uow = new UnitOfWork();
        //        if (!ModelState.IsValid)
        //        {
        //            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
        //            model.SessionTypes = GetSessionTypes();
        //            TempData["Error"] = Resources.Resources.LblMissingData;
        //            return RedirectToAction("proposal", "problem", new { id = model.BidId });
        //        }
        //        else
        //        {
        //            bool record = false;
        //            if (model.Record == "1" && model.SessionType == 1)
        //            {
        //                record = true;
        //            }
        //            string dateAndTime = model.Date;
        //            DateTime classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            if (Convert.ToInt32(model.DurationHour) > 0 || Convert.ToInt32(model.DurationMinutes) > 0)
        //            {
        //                dateAndTime = model.Date + " " + model.ClassHour + ":" + model.ClassMinute + " " + model.ClassAMPM;
        //                classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
        //            }
        //            //first create braincert class
        //            int brainCertId = 0;
        //            if (model.SessionType == 1)
        //            {
        //                string classEndTime = classDate.AddHours(Convert.ToDouble(model.DurationHour)).AddMinutes(Convert.ToDouble(model.DurationMinutes)).ToString("HH:mm tt");
        //                model.StartTime = classDate.ToString("hh:mm tt");
        //                // only 30 minute class is allowed in trial period
        //                model.ClassEndTime = classDate.AddMinutes(30).ToString("hh:mm tt"); //classEndTime;                        
        //                brainCertId = BrainCert.CreateBrainCertClass(model.Title, classDate.ToString("MM/dd/yyyy HH:mm"), model.StartTime, model.ClassEndTime, Convert.ToInt32(model.Record), Convert.ToInt32(model.TimeZone));
        //                if (brainCertId == 0)
        //                {
        //                    TempData["Error"] = Resources.Resources.MsgErrorTryAgain;
        //                    return RedirectToAction("proposal", "problem", new { id = model.BidId });
        //                }
        //            }
        //            Class clsCreate = new Class()
        //            {
        //                ClassID = Guid.NewGuid().ToString(),
        //                Title = model.Title,
        //                ClassDate = classDate,
        //                StartTime = model.StartTime,
        //                EndTime = model.ClassEndTime,
        //                Duration = model.Duration,
        //                CreationDate = DateTime.Now,
        //                Type = model.SessionType,
        //                Record = record,
        //                CreatedBy = Session["UserId"].ToString(),
        //                TeacherID = model.TeacherID,
        //                Description = model.Description,
        //                SubjectID = model.Subject,
        //                ProblemID = model.ProblemID,
        //                Status = (int)ClassStatus.BidAccepted,
        //                CreatedByStudent = true,
        //                TimeZone = model.TimeZone
        //                //BrainCertId = (int)model.BrainCertId
        //            };
        //            uow.Classes.Insert(clsCreate);
        //            uow.Save();
        //            UpdateProposalStatus(model.BidId, "accept");
        //            DeclineAllOtherProposals(model.ProblemID, model.BidId);
        //            TempData["Message"] = Resources.Resources.MsgClassCreatedSuccess;
        //            return RedirectToAction("proposal", "problem", new { id = model.BidId });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData["Message"] = Resources.Resources.MsgErrorTryAgain;
        //        return RedirectToAction("proposal", "problem", new { id = model.BidId });
        //    }
        //}

        public void DeclineAllOtherProposals(string problemId, string bidId)
        {
            string userId = Session["UserId"].ToString();
            UnitOfWork uow = new UnitOfWork();
            List<StudentProblemBid> allBids = uow.StudentProblemBids.Get(x => x.ProblemID == problemId && x.BidID != bidId).ToList();
            foreach (var bid in allBids)
            {
                bid.Status = (int)BidStatus.Declined;
                Common.AddNotification("Your proposal has been declined.", "", userId, bid.UserID, "/bid/detail/" + bid.BidID, (int)NotificationType.Bid);
                //send decline message
                Message msg = new Message
                {
                    BidID = bid.BidID,
                    FromUser = userId,
                    ToUser = bid.UserID,
                    CreationDate = DateTime.Now,
                    Message1 = "Your proposal has been declined by " + Session["UserName"].ToString(),
                    Status = 1,
                };
                uow.Messages.Insert(msg);
                uow.Save();
            }
            uow.Save();
            uow.Dispose();
        }

        [HttpPost]
        public ActionResult CompleteClass(string classId, string bidId)
        {
            UnitOfWork uow = new UnitOfWork();
            Class classDetail = uow.Classes.Get(x => x.ClassID == classId).FirstOrDefault();
            classDetail.Status = (int)ClassStatus.Completed;
            uow.Save();
            UpdateTutorWallet(classDetail.TeacherID, classId, (decimal)classDetail.Duration);
            Common.AddNotification(Session["UserName"].ToString() + " marked the class as complete.", "", Session["UserId"].ToString(), classDetail.TeacherID, "/tutor/classes", (int)NotificationType.Class);
            return RedirectToAction("proposal", "problem", new { id = bidId });
        }

        public void UpdateTutorWallet(string teacherId, string classId, decimal hours)
        {
            UnitOfWork uow = new UnitOfWork();
            ClassPrice subjectPrice = uow.TeacherRepository.GetClassSubjectRate(classId);
            Wallet wallet = uow.Wallets.Get(x => x.UserID == teacherId).FirstOrDefault();
            if (wallet != null)
            {
                wallet.Hours = hours;
                wallet.Funds = hours * subjectPrice.HourlyRate;
                wallet.Status = 1;
                wallet.TransactionType = "Credit";
                wallet.UserID = teacherId;
            }
            else
            {
                wallet = new Wallet();
                wallet.Hours = hours;
                wallet.Funds = hours * subjectPrice.HourlyRate;
                wallet.Status = 1;
                wallet.TransactionType = "Credit";
                wallet.UserID = teacherId;
                wallet.CreationDate = DateTime.Now;
                uow.Wallets.Insert(wallet);
            }

            //insert wallet log
            TeacherCreditLog creditLog = new TeacherCreditLog
            {
                ClassID = classId,
                TeacherID = teacherId,
                CreationDate = DateTime.Now,
                CreditsEarned = hours,
                Funds = (decimal)(hours * subjectPrice.HourlyRate),
                LogType = "Credit"
            };
            uow.TeacherCreditLogs.Insert(creditLog);
            uow.Save();
            uow.Dispose();
            Common.AddNotification("Your wallet has beend credited with " + Math.Round(creditLog.CreditsEarned, 2) + " hours", "",
                            "admin", teacherId, "/tutor/wallet", (int)NotificationType.Class);
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