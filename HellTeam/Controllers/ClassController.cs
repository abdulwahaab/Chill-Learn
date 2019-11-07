using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizationFilter]
    public class ClassController : Controller
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

        public ActionResult Create()
        {
            UnitOfWork uow = new UnitOfWork();
            ClassViewModel classView = new ClassViewModel();
            classView.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            classView.SessionTypes = GetSessionTypes();
            return View(classView);
        }

        [HttpPost]
        public ActionResult Create(ClassViewModel model)
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
                    bool record = false;
                    if(model.Record == "1")
                    {
                        record = true;
                    }
                    Class clsCreate = new Class()
                    {
                        ClassID = Guid.NewGuid().ToString(),
                        Title = model.Title,
                        ClassFrom = Convert.ToDateTime(model.Date),
                        ClassTo = model.Time,
                        Duration = model.Duration,
                        CreationDate = DateTime.Now,
                        Type = model.SessionType,
                        Record = record,
                        CreatedBy = Session["UserId"].ToString(),
                        TeacherID = Session["UserId"].ToString(),
                        Description = model.Description,
                        SubjectID = model.Subject,
                        Status = (int)ClassStatus.Pending
                    };
                    uow.Classes.Insert(clsCreate);
                    uow.Save();
                    ClassViewModel classView = new ClassViewModel();
                    classView.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                    classView.SessionTypes = GetSessionTypes();
                    ModelState.AddModelError("success", "Class Created Successfully.");
                    return View(classView);
                }
            }
            catch (Exception)
            {
                return View();
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
        public bool Join(JoinParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                StudentClass studentClass = new StudentClass
                {
                    ClassID = model.ClassId,
                    JoiningDate = DateTime.Now,
                    StudentID = Session["UserId"].ToString(),
                    Status = (int)ClassStatus.Pending
                };
                uow.StudentClasses.Insert(studentClass);
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