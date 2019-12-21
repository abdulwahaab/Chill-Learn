using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;

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
                List<Subject> subjects = uow.Subjects.Get().ToList();
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
                classView.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
                classView.SessionTypes = GetSessionTypes();
                return View(classView);
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
                if (!ModelState.IsValid)
                {
                    model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                    model.SessionTypes = GetSessionTypes();
                    return View(model);
                }
                else
                {
                    bool record = false;
                    if (model.Record == "1")
                    {
                        record = true;
                    }
                    if (!string.IsNullOrEmpty(model.ClassID))
                    {
                        //Class classDetail = uow.Classes.Get(x=> x.ClassID== model.ClassID).FirstOrDefault();
                    }
                    else
                    {
                        string datae = model.Date + " " + model.Time.Insert(model.Time.Length - 2, " ");
                        DateTime combDT = DateTime.ParseExact(datae, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                        Class clsCreate = new Class()
                        {
                            ClassID = Guid.NewGuid().ToString(),
                            Title = model.Title,
                            //ClassDate = DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ClassDate = combDT,
                            ClassTime = model.Time,
                            Duration = model.Duration,
                            CreationDate = DateTime.Now,
                            Type = model.SessionType,
                            Record = record,
                            CreatedBy = Session["UserId"].ToString(),
                            TeacherID = Session["UserId"].ToString(),
                            Description = model.Description,
                            SubjectID = model.Subject,
                            Status = (int)ClassStatus.Pending,
                            BrainCertId = model.BrainCertId,
                            CreatedByStudent = false
                        };
                        uow.Classes.Insert(clsCreate);
                        uow.Save();
                        AddClassFiles(model.files, clsCreate.ClassID);
                    }
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
        public bool Join(ClassActionParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                Class classs = uow.Classes.Get().Where(a => a.ClassID == model.ClassId).FirstOrDefault();
                StudentCredit studentCredit = uow.StudentCredits.Get().Where(a => a.StudentID == Session["UserId"].ToString()).FirstOrDefault();
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public bool Cancel(ClassActionParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                Class cls = uow.Classes.Get().Where(a => a.ClassID == model.ClassId).FirstOrDefault();
                if (cls != null)
                {
                    cls.Status = (int)ClassStatus.Cancelled;
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
        public async System.Threading.Tasks.Task<ActionResult> Edit(ClassEditModel model)
        {
            //using (var client = new HttpClient())
            //{
            //    var response = await client.PostAsync("https://api.braincert.com/v2/getclasslaunch?apikey=EBqafLB3sAk1HeCDxr4Z&class_id="+model.BrainCertId+ "&userId=1122&isTeacher=1&&userName=Faisal&lessonName=hha&courseName=dfdf", null);
            //    response.EnsureSuccessStatusCode();
            //    string responseBody = await response.Content.ReadAsStringAsync();
            //}

            UnitOfWork uow = new UnitOfWork();
            Class cls = uow.Classes.Get().Where(a => a.Id == model.Id).FirstOrDefault();
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
                cls.ClassTime = model.ClassTime;
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

        public async System.Threading.Tasks.Task<ActionResult> Detail(string c)
        {
            UnitOfWork uow = new UnitOfWork();
            ClassEditModel model = uow.TeacherRepository.GetClassData(c);
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://api.braincert.com/v2/getclassreport?apikey=EBqafLB3sAk1HeCDxr4Z&format=json&classId=" + model.BrainCertId + "", null);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<AttendenceReport> myProduct = JsonConvert.DeserializeObject<List<AttendenceReport>>(responseBody);

                List<AttendenceReport> students = myProduct.Where(a => a.isTeacher == 0).ToList();
                List<AttendenceReportModel> listRep = new List<AttendenceReportModel>();
                for (int i = 0; i < students.Count; i++)
                {
                    AttendenceReportModel attenReport = uow.TeacherRepository.GetUserInfo(students[i].userId, (int)ClassJoinStatus.Approved);
                    //var timeSpan = TimeSpan.FromHours(Convert.ToDouble(attenReport.CreditsUsed));
                    decimal dec = Convert.ToDecimal(TimeSpan.Parse(students[i].duration).TotalHours);
                    attenReport.CreditsConsumed = String.Format("{0:0.00}", dec);
                    attenReport.CreditsRefund = String.Format("{0:0.00}", attenReport.CreditsUsed - dec);
                    listRep.Add(attenReport);
                }
                ViewBag.Attendence = listRep;
            }

            return View(model);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ProcessAttendence(string c)
        {
            UnitOfWork uow = new UnitOfWork();
            ClassEditModel model = uow.TeacherRepository.GetClassData(c);
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://api.braincert.com/v2/getclassreport?apikey=EBqafLB3sAk1HeCDxr4Z&format=json&classId=" + model.BrainCertId + "", null);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<AttendenceReport> myProduct = JsonConvert.DeserializeObject<List<AttendenceReport>>(responseBody);

                List<AttendenceReport> students = myProduct.Where(a => a.isTeacher == 0).ToList();
                List<AttendenceReportModel> listRep = new List<AttendenceReportModel>();
                for (int i = 0; i < students.Count; i++)
                {
                    AttendenceReportModel attenReport = uow.TeacherRepository.GetUserInfo(students[i].userId, (int)ClassJoinStatus.Approved);
                    if (attenReport != null)
                    {
                        decimal dec = Convert.ToDecimal(TimeSpan.Parse(students[i].duration).TotalHours);
                        attenReport.CreditsConsumed = String.Format("{0:0.00}", dec);
                        attenReport.CreditsRefund = String.Format("{0:0.00}", attenReport.CreditsUsed - dec);
                        attenReport.CreditsConsumedInt = dec;
                        attenReport.CreditsRefundInt = attenReport.CreditsUsed - dec;
                        attenReport.StudentClassId = students[i].userId;
                        listRep.Add(attenReport);
                    }
                }
                for (int i = 0; i < listRep.Count; i++)
                {
                    StudentClass studentClass = uow.StudentClasses.Get().Where(a => a.ID == listRep[i].StudentClassId).FirstOrDefault();
                    if (studentClass != null)
                    {
                        studentClass.Status = (int)ClassJoinStatus.Processed;
                        uow.StudentClasses.Update(studentClass);
                        if (listRep[i].CreditsConsumedInt > 0)
                        {

                        }
                    }
                }
                uow.Save();
            }

            return View(model);
        }
    }
}