using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using ChillLearn.DAL;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Globalization;
using ChillLearn.ViewModels;
using System.Threading.Tasks;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizationFilter]
    public class ClassController : BaseController
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
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

        //[Filters.ApprovedFilter]
        public ActionResult Create(string id)
        {
            if ((int)Session["UserStatus"] != (int)UserStatus.Approved)
            {
                ViewBag.IsApproved = false;
                return View();
            }
            else
            {
                ViewBag.IsApproved = true;
                UnitOfWork uow = new UnitOfWork();
                ClassViewModel classView = new ClassViewModel();

                if (!string.IsNullOrEmpty(id))
                {
                    Class classDetail = uow.Classes.Get(x => x.ClassID == id).FirstOrDefault();
                    if (classDetail != null)
                    {
                        classView.ClassID = classDetail.ClassID;
                        classView.Title = classDetail.Title;
                        classView.Record = classDetail.Record.ToString();
                    }
                }
                return View(ReturnCreateClassView(classView));
            }
        }

        [HttpPost]
        [Filters.ApprovedFilter]
        public ActionResult Create(ClassViewModel model)
        {
            try
            {
                ViewBag.IsApproved = true;
                UnitOfWork uow = new UnitOfWork();
                if (Convert.ToInt32(model.DurationHour) < 1 && Convert.ToInt32(model.DurationMinutes) < 30)
                {
                    ModelState.AddModelError("classtime-error", Resources.Resources.MsgClassDurationError);
                    return View(ReturnCreateClassView(model));
                }
                else if (!ModelState.IsValid)
                {
                    return View(ReturnCreateClassView(model));
                }
                else
                {
                    bool record = false;
                    if (model.Record == "1" && model.SessionType == 1)
                        record = true;
                    string dateAndTime = model.Date;
                    DateTime classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (Convert.ToInt32(model.DurationHour) > 0 || Convert.ToInt32(model.DurationMinutes) > 0)
                    {
                        //dateAndTime = model.Date + " " + model.StartTime.Insert(model.StartTime.Length - 2, " ");
                        dateAndTime = model.Date + " " + model.ClassHour + ":" + model.ClassMinute + " " + model.ClassAMPM; //.Insert(model.StartTime.Length - 2, " ");
                        classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    }
                    //first create braincert class
                    int brainCertId = 0;
                    if (model.SessionType == 1)
                    {
                        string classEndTime = classDate.AddHours(Convert.ToDouble(model.DurationHour)).AddMinutes(Convert.ToDouble(model.DurationMinutes)).ToString("HH:mm tt");
                        model.StartTime = classDate.ToString("hh:mm tt");
                        // only 30 minute class is allowed in trial period
                        model.ClassEndTime = classDate.AddMinutes(30).ToString("hh:mm tt"); //classEndTime;                        
                        brainCertId = BrainCert.CreateBrainCertClass(model.Title, classDate.ToString("MM/dd/yyyy HH:mm"), model.StartTime, model.ClassEndTime, Convert.ToInt32(model.Record), Convert.ToInt32(model.TimeZone));
                        if (brainCertId == 0)
                        {
                            ModelState.AddModelError("error", Resources.Resources.MsgErrorTryAgain);
                            return View(ReturnCreateClassView(model));
                        }
                    }
                    decimal classDuration = Math.Round(Convert.ToInt16(model.DurationHour) + (Convert.ToInt16(model.DurationMinutes) / 60m), 2);
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
                        TeacherID = Session["UserId"].ToString(),
                        Description = model.Description,
                        SubjectID = model.Subject,
                        Status = (int)ClassStatus.Created,
                        BrainCertId = brainCertId,
                        CreatedByStudent = false
                    };
                    uow.Classes.Insert(clsCreate);
                    uow.Save();
                    AddClassFiles(model.files, clsCreate.ClassID);
                    //send to invite page in case of written class
                    if (model.SessionType == (int)SessionType.Written)
                        return RedirectToAction("Invite", new { id = clsCreate.ClassID });
                    ClassViewModel classView = new ClassViewModel();
                    ModelState.Clear();
                    ModelState.AddModelError("success", Resources.Resources.MsgClassCreatedSuccess);
                    return View(ReturnCreateClassView(classView));
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ClassViewModel ReturnCreateClassView(ClassViewModel model)
        {
            Common common = new Common();
            UnitOfWork uow = new UnitOfWork();
            model.DurationHourList = new SelectList(common.GetDurationHours());
            model.DurationMinuteList = new SelectList(common.GetMinutes());
            model.HourList = new SelectList(common.GetHours());
            model.MinuteList = new SelectList(common.GetMinutes());
            model.AMPMList = new SelectList(common.GetAMPM());
            model.TimeZones = new SelectList(uow.TimeZones.Get(), "GMT", "Name");
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypes();
            return model;
        }

        public void AddClassFiles(HttpPostedFileBase[] files, string classId)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        //Upload to class folder
                        var fileExe = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Content/images/class/") + fileExe);
                        file.SaveAs(ServerSavePath);
                        //add to database
                        ClassFile clsFile = new ClassFile();
                        clsFile.ClassId = classId;
                        clsFile.Image = fileExe;
                        uow.ClassFiles.Insert(clsFile);
                    }
                }
                uow.Save();
            }
            catch (Exception ex)
            {

            }

        }

        public ActionResult Find(string q, string s, string t)
        {
            ViewBag.Keyword = q;
            ViewBag.Subject = s;
            ViewBag.Teacher = t;
            int subjectId = 0;
            if (!string.IsNullOrEmpty(s))
                subjectId = Convert.ToInt32(s);
            UnitOfWork uow = new UnitOfWork();
            ClassFindParam model = new ClassFindParam();
            string dateNow;
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null && cultureCookie.Value == "ar-SA")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                dateNow = DateTime.Now.ToString();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
            }
            else
                dateNow = DateTime.Now.ToString();
            model.Classes = uow.TeacherRepository.SearchClasses(subjectId, t, 0, Session["UserId"].ToString(), (int)ClassStatus.Cancelled, dateNow, q);
            model.Teachers = new SelectList(uow.UserRepository.GetUserByType((int)UserRoles.Teacher), "UserId", "UserName");
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypes();
            return View(model);
        }

        [HttpPost]
        public ActionResult Find(ClassFindParam model)
        {
            UnitOfWork uow = new UnitOfWork();
            var search = model.Search;
            ViewBag.Keyword = search.q;
            string dateNow;
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null && cultureCookie.Value == "ar-SA")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                dateNow = DateTime.Now.ToString();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
            }
            else
                dateNow = DateTime.Now.ToString();
            model.Classes = uow.TeacherRepository.SearchClasses(search.SubjectId, search.TeacherId, 0, Session["UserId"].ToString(), (int)ClassStatus.Cancelled, dateNow, search.q);
            model.Teachers = new SelectList(uow.UserRepository.GetUserByType((int)UserRoles.Teacher), "UserId", "UserName");
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypes();
            return View(model);
        }

        [HttpPost]
        public string Join(ClassActionParam model)
        {
            try
            {
                string userId = Session["UserId"].ToString();
                UnitOfWork uow = new UnitOfWork();
                Class classs = uow.Classes.Get(a => a.ClassID == model.ClassId).FirstOrDefault();
                StudentCredit studentCredit = uow.StudentCredits.Get(a => a.StudentID == userId).FirstOrDefault();
                if (classs != null && studentCredit != null)
                {
                    if (studentCredit.TotalCredits > classs.Duration)
                    {
                        StudentClass studentClass = new StudentClass
                        {
                            ClassID = model.ClassId,
                            JoiningDate = DateTime.Now,
                            StudentID = Session["UserId"].ToString(),
                            Status = (int)ClassJoinStatus.Pending
                        };
                        uow.StudentClasses.Insert(studentClass);
                        uow.Save();
                        Common.AddNotification(Session["UserName"] + " requested to join your class " + classs.Title, "", studentClass.StudentID, classs.TeacherID, "/tutor/requests?c=" + classs.ClassID, (int)NotificationType.Class);
                        return "success";
                    }
                    else
                    {
                        return "no-balance";
                    }
                }
                return "no-balance";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [HttpPost]
        public bool Cancel(ClassActionParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                Class cls = uow.Classes.Get(a => a.ClassID == model.ClassId).FirstOrDefault();
                if (cls != null)
                {
                    cls.Status = (int)ClassStatus.Cancelled;
                    cls.CancelReason = model.CancelReason;
                    cls.UpdateDate = DateTime.Now;
                }
                uow.Classes.Update(cls);
                uow.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ActionResult Edit(string c)
        {
            UnitOfWork uow = new UnitOfWork();
            ViewBag.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            ViewBag.SessionTypes = GetSessionTypes();
            ClassEditModel model = uow.TeacherRepository.GetClassData(c);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ClassEditModel model)
        {
            //using (var client = new HttpClient())
            //{
            //    var response = await client.PostAsync("https://api.braincert.com/v2/getclasslaunch?apikey=EBqafLB3sAk1HeCDxr4Z&class_id="+model.BrainCertId+ "&userId=1122&isTeacher=1&&userName=Faisal&lessonName=hha&courseName=dfdf", null);
            //    response.EnsureSuccessStatusCode();
            //    string responseBody = await response.Content.ReadAsStringAsync();
            //}

            UnitOfWork uow = new UnitOfWork();
            Class cls = uow.Classes.Get(a => a.Id == model.Id).FirstOrDefault();
            if (cls != null)
            {
                bool record = false;
                if (model.Record == "1")
                {
                    record = true;
                }
                string datae = model.ClassDate + " " + model.ClassTime.Insert(model.ClassTime.Length - 2, " ");
                DateTime combDT = DateTime.ParseExact(datae, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                cls.SubjectID = model.SubjectId;
                cls.Title = model.Title;
                cls.ClassDate = combDT;
                cls.StartTime = model.ClassTime;
                cls.Description = model.Description;
                cls.Duration = model.Duration;
                cls.Type = model.Type;
                cls.UpdateDate = DateTime.Now;
                cls.Record = record;
                //uow.Classes.Update(cls);
                //uow.Save();
            }

            ViewBag.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            ViewBag.SessionTypes = GetSessionTypes();

            return View(model);
        }

        public ActionResult Detail(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            ClassEditModel model = uow.TeacherRepository.GetClassData(id);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync("https://api.braincert.com/v2/getclassreport?apikey=EBqafLB3sAk1HeCDxr4Z&format=json&classId=" + model.BrainCertId + "", null).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                if (!responseBody.Contains("error"))
                {
                    List<AttendanceReport> myProduct = JsonConvert.DeserializeObject<List<AttendanceReport>>(responseBody);
                    List<AttendanceReport> students = myProduct.Where(a => a.isTeacher == 0).ToList();
                    List<AttendenceReportModel> listRep = new List<AttendenceReportModel>();
                    for (int i = 0; i < students.Count; i++)
                    {
                        AttendenceReportModel attenReport = uow.TeacherRepository.GetUserInfo(students[i].userId, id);
                        if (attenReport != null)
                        {
                            decimal dec = Convert.ToDecimal(TimeSpan.Parse(students[i].duration).TotalHours);
                            attenReport.CreditsConsumed = String.Format("{0:0.00}", dec);
                            attenReport.CreditsRefund = String.Format("{0:0.00}", attenReport.CreditsUsed - dec);
                            listRep.Add(attenReport);
                        }
                    }
                    ViewBag.Attendence = listRep;
                }
                else
                    return RedirectToAction("classes", "tutor");
            }
            return View(model);
        }

        public ActionResult ProcessClass(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            Class classDetail = uow.Classes.Get(x => x.ClassID == id).FirstOrDefault();
            if (classDetail != null && classDetail.Status != (int)ClassStatus.Completed)
            {
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync("https://api.braincert.com/v2/getclassreport?apikey=EBqafLB3sAk1HeCDxr4Z&format=json&classId=" + classDetail.BrainCertId + "", null).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    List<AttendanceReport> report = JsonConvert.DeserializeObject<List<AttendanceReport>>(responseBody);
                    AttendanceReport studentReport = report.Where(a => a.isTeacher == 0).ToList().FirstOrDefault();
                    User student = uow.Users.Get(x => x.AutoID == studentReport.userId).FirstOrDefault();
                    string studentId = student.UserID;
                    AttendenceReportModel attenReport = uow.TeacherRepository.GetUserInfo(studentReport.userId, id);
                    if (attenReport != null)
                    {
                        decimal classTime = Convert.ToDecimal(TimeSpan.Parse(studentReport.duration).TotalHours);
                        attenReport.CreditsConsumed = String.Format("{0:0.00}", classTime);
                        attenReport.CreditsRefund = String.Format("{0:0.00}", attenReport.CreditsUsed - classTime);
                        attenReport.CreditsConsumedInt = classTime;
                        attenReport.CreditsRefundInt = attenReport.CreditsUsed - classTime;
                        attenReport.StudentClassId = studentReport.userId;
                        //mark class as completed
                        classDetail.Status = (int)ClassStatus.Completed;
                        StudentCreditLog creditLog = new StudentCreditLog
                        {
                            ClassID = classDetail.ClassID,
                            UserID = id,
                            CreationDate = DateTime.Now,
                            CreditsUsed = Math.Round(attenReport.CreditsRefundInt, 2),
                            LogType = "Refund"
                        };
                        uow.StudentCreditLogs.Insert(creditLog);
                        StudentCredit credit = uow.StudentCredits.Get(a => a.StudentID == studentId).FirstOrDefault();
                        if (credit != null)
                        {
                            credit.LastUpdates = DateTime.Now;
                            credit.TotalCredits = credit.TotalCredits + attenReport.CreditsRefundInt;
                            credit.UsedCredits = credit.UsedCredits - attenReport.CreditsRefundInt;
                        }
                        UpdateTutorWallet(classDetail.TeacherID, classDetail.ClassID, classTime);
                        //payment refund notification
                        Common.AddNotification("Your wallet has beend refunded with " + creditLog.CreditsUsed, "",
                            Session["UserId"].ToString(), studentId, "/student/wallet", (int)NotificationType.Refund);
                    }
                    uow.Save();
                }
            }
            return RedirectToAction("classes", "tutor");
        }

        public ActionResult ProcessAttendence(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            Class classModel = uow.Classes.Get(x => x.ClassID == id).FirstOrDefault();
            if (classModel != null && classModel.Status != (int)ClassStatus.Completed)
            {
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync("https://api.braincert.com/v2/getclassreport?apikey=EBqafLB3sAk1HeCDxr4Z&format=json&classId=" + classModel.BrainCertId + "", null).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    List<AttendanceReport> myProduct = JsonConvert.DeserializeObject<List<AttendanceReport>>(responseBody);
                    decimal teacherClassTime = Convert.ToDecimal(TimeSpan.Parse(myProduct.Where(a => a.isTeacher == 1).FirstOrDefault().duration).TotalHours);
                    List<AttendanceReport> students = myProduct.Where(a => a.isTeacher == 0).ToList();
                    List<AttendenceReportModel> listRep = new List<AttendenceReportModel>();
                    bool IsProcessed = false;
                    for (int i = 0; i < students.Count; i++)
                    {
                        AttendenceReportModel attenReport = uow.TeacherRepository.GetUserInfo(students[i].userId, id);
                        if (attenReport != null)
                        {
                            IsProcessed = true;
                            decimal dec = Convert.ToDecimal(TimeSpan.Parse(students[i].duration).TotalHours);
                            attenReport.CreditsConsumed = String.Format("{0:0.00}", dec);
                            attenReport.CreditsRefund = String.Format("{0:0.00}", attenReport.CreditsUsed - dec);
                            attenReport.CreditsConsumedInt = dec;
                            attenReport.CreditsRefundInt = attenReport.CreditsUsed - dec;
                            attenReport.StudentClassId = students[i].userId;
                            listRep.Add(attenReport);
                        }
                    }
                    if (IsProcessed == true)
                    {
                        for (int i = 0; i < listRep.Count; i++)
                        {
                            int studentAutoID = listRep[i].StudentClassId;
                            User student = uow.Users.Get(x => x.AutoID == studentAutoID).FirstOrDefault();
                            StudentClass studentClass = uow.StudentClasses.Get(a => a.StudentID == student.UserID && a.ClassID == id).FirstOrDefault();
                            if (studentClass != null)
                            {
                                studentClass.Status = (int)ClassJoinStatus.Processed;
                                uow.StudentClasses.Update(studentClass);
                                StudentCreditLog creditLog = new StudentCreditLog
                                {
                                    ClassID = id,
                                    UserID = studentClass.StudentID,
                                    CreationDate = DateTime.Now,
                                    CreditsUsed = Math.Round(listRep[i].CreditsRefundInt, 2),
                                    LogType = "Refund"
                                };
                                uow.StudentCreditLogs.Insert(creditLog);
                                StudentCredit credit = uow.StudentCredits.Get(a => a.StudentID == studentClass.StudentID).FirstOrDefault();
                                if (credit != null)
                                {
                                    credit.LastUpdates = DateTime.Now;
                                    credit.TotalCredits = credit.TotalCredits + listRep[i].CreditsRefundInt;
                                    credit.UsedCredits = credit.UsedCredits - listRep[i].CreditsRefundInt;
                                    uow.StudentCredits.Update(credit);
                                }
                                UpdateTutorWallet(classModel.TeacherID, classModel.ClassID, teacherClassTime);
                                //payment refund notification
                                Common.AddNotification("Your wallet has beend refunded with " + creditLog.CreditsUsed, "",
                                    Session["UserId"].ToString(), studentClass.StudentID, "/student/wallet", (int)NotificationType.Refund);
                            }
                        }
                    }
                    uow.Save();
                }
            }
            return RedirectToAction("classes", "tutor");
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
            Common.AddNotification("Your wallet has beend credited with " + Math.Round(creditLog.CreditsEarned, 2), "",
                            "admin", Session["UserId"].ToString(), "/tutor/wallet", (int)NotificationType.Class);
        }

        public ActionResult Invite(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            ClassEditModel classDetail = uow.TeacherRepository.GetClassData(id);
            InviteStudentModel model = new InviteStudentModel();
            model.ClassTitle = classDetail.Title;
            model.ClassID = classDetail.ClassId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Invite(InviteStudentModel model)
        {
            UnitOfWork uow = new UnitOfWork();
            List<User> searcResults = uow.Users.Get(x => x.UserRole == (int)UserRoles.Student && x.FirstName.Contains(model.SearchKeyword) ||
            x.LastName.Contains(model.SearchKeyword) || x.Email.Contains(model.SearchKeyword)).ToList();
            model.Students = searcResults;
            ClassEditModel classDetail = uow.TeacherRepository.GetClassData(model.ClassID);
            model.ClassTitle = classDetail.Title;
            model.ClassID = classDetail.ClassId;
            if (!string.IsNullOrEmpty(model.StudentID))
                if (CreateInviteStudentClass(model.StudentID, model.ClassID, model.ClassTitle))
                    ModelState.AddModelError("success", Resources.Resources.MsgStudentInvited);
                else
                    ModelState.AddModelError("success", Resources.Resources.LblMultipleStudentError);

            ModelState.AddModelError("success", Resources.Resources.MsgStudentInvited);
            return View(model);
        }

        public bool CreateInviteStudentClass(string studentId, string classId, string className)
        {
            UnitOfWork uow = new UnitOfWork();

            StudentClass studentClass = uow.StudentClasses.Get(x => x.ClassID == classId && x.StudentID == studentId).FirstOrDefault();
            if (studentClass == null)
            {
                studentClass = new StudentClass();
                studentClass.ClassID = classId;
                studentClass.StudentID = studentId;
                studentClass.JoiningDate = DateTime.Now;
                studentClass.Status = (int)ClassJoinStatus.Invited;
                uow.StudentClasses.Insert(studentClass);
                uow.Save();
                Common.AddNotification(Session["UserName"].ToString() + " invited you to class " + className, "", Session["UserId"].ToString(),
                    studentId, "/student/classes", (int)NotificationType.Class);
                return true;
            }
            return false;
        }
    }
}