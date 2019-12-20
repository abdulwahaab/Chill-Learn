using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
                        Class clsCreate = new Class()
                        {
                            ClassID = Guid.NewGuid().ToString(),
                            Title = model.Title,
                            ClassDate = DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                            BrainCertId = model.BrainCertId
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
            catch (Exception)
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

        public ActionResult Find()
        {
            UnitOfWork uow = new UnitOfWork();
            ClassFindParam model = new ClassFindParam();
            model.Classes = uow.TeacherRepository.SearchClasses(0, "", 0, Session["UserId"].ToString());
            model.Teachers = new SelectList(uow.UserRepository.GetUserByType((int)UserRoles.Teacher), "UserId", "UserName");
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypes();
            return View(model);
        }

        [HttpPost]
        public ActionResult Find(ClassFindParam model)
        {
            UnitOfWork uow = new UnitOfWork();
            var s = model.Search;
            model.Classes = uow.TeacherRepository.SearchClasses(s.SubjectId, s.TeacherId, s.SessionType, Session["UserId"].ToString());
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
                if (classs != null)
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
    }
}