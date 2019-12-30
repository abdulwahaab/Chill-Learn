using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.Data.Models;
using ChillLearn.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeStudent]
    public class StudentController : BaseController
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

        public ActionResult CreateProblem(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            ProblemsModel model = new ProblemsModel();
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            model.SessionTypes = GetSessionTypess();
            model.TeacherID = id;
            if (!string.IsNullOrEmpty(id))
            {
                User selectedTeacher = uow.Users.GetByID(id);
                ViewBag.TeacherName = selectedTeacher.FirstName + " " + selectedTeacher.LastName;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateProblem(ProblemsModel model, List<HttpPostedFileBase> files)
        {
            UnitOfWork uow = new UnitOfWork();
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypess();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
                return View(model);
            }
            string userId = Session["UserId"].ToString();
            StudentCredit studentCredit = uow.StudentCredits.Get(x => x.StudentID == userId).FirstOrDefault();
            if (studentCredit != null)
            {
                decimal balanceHours = (decimal)(uow.StudentCredits.Get(x => x.StudentID == userId).FirstOrDefault().TotalCredits);
                if (balanceHours > model.HoursNeeded)
                {
                    StudentProblem problem = new StudentProblem
                    {
                        ProblemID = Guid.NewGuid().ToString(),
                        StudentID = Session["UserId"].ToString(),
                        SubjectID = model.Subject,
                        CreationDate = DateTime.Now,
                        Description = model.ProblemDescription,
                        HoursNeeded = model.HoursNeeded,
                        Type = model.Type,
                        //FileName = fileName,
                        TeacherID = model.TeacherID,
                        Status = model.TeacherID != null ? (int)ProblemStatus.TeacherSelected : (int)ProblemStatus.Created,
                        ExpireDate = DateTime.ParseExact(model.DeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture) // need to add datetime datepicker
                    };
                    uow.StudentProblems.Insert(problem);
                    //save problem files(s) in uploads folder
                    if (files != null && files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            if (file != null)
                            {
                                string fileName = null;
                                //fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
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
                                //save problem files(s) in database
                                StudentProblemFile problemFile = new StudentProblemFile
                                {
                                    ProblemID = problem.ProblemID,
                                    FileName = fileName,
                                    CreationDate = DateTime.Now,
                                    UserID = userId
                                };
                                uow.StudentProblemFiles.Insert(problemFile);
                            }
                        }
                    }
                    uow.Save();
                    //add notification if teacher is selected
                    if (!string.IsNullOrEmpty(model.TeacherID))
                        Common.AddNotification(Session["UserName"] + " asked you a question", "", Session["UserId"].ToString(), model.TeacherID, "/tutor/writeproposal?q=" + problem.ProblemID, (int)NotificationType.Question);
                    //end
                    ModelState.AddModelError("success", Resources.Resources.MsgProblemSubmitedSuccessfully);
                    return View(model);
                }
                else
                {
                    model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                    model.SessionTypes = GetSessionTypess();
                    ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
                    return View(model);
                }
            }
            else
            {
                model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
                model.SessionTypes = GetSessionTypess();
                ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
                return View(model);
            }
        }

        public ActionResult Problems()
        {
            UnitOfWork uow = new UnitOfWork();
            ProblemsModel model = new ProblemsModel();
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
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
                ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
                return View(model);
            }
            string fileName = null;
            if (file != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                file.SaveAs(path);
                //byte[] bytes;
                //using (BinaryReader br = new BinaryReader(file.InputStream))
                //{
                //    bytes = br.ReadBytes(file.ContentLength);
                //}
                //fileName = bytes;

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
                ExpireDate = DateTime.ParseExact(model.DeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture) // need to add datetime datepicker
            };
            uow.StudentProblems.Insert(problem);
            uow.Save();
            model.Problems = uow.UserRepository.GetProblemsByStudentId(Session["UserId"].ToString());
            ModelState.AddModelError("success", Resources.Resources.MsgProblemSubmitedSuccessfully);
            return View(model);
        }

        public ActionResult Problem(string id)
        {
            if (id != null)
            {
                UnitOfWork uow = new UnitOfWork();
                ProblemDetailModel model = new ProblemDetailModel();
                model.ProblemDetails = uow.UserRepository.GetQuestionDetailById(id);
                if (model.ProblemDetails != null)
                {
                    model.ProblemId = id;
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
        public ActionResult Problem(ProblemDetailModel model)
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
            return RedirectToAction("problem", "student", new { problem = model.ProblemId });
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

        public new ActionResult Profile()
        {
            UserService userService = new UserService();
            User user = new User();
            var userId = Session["UserId"];
            user = userService.GetProfile(userId.ToString());
            if (user != null)
            {
                ProfileModel profile = new ProfileModel
                {
                    UserId = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = Encryptor.Decrypt(user.Email),
                    //Picture = user.Picture,
                    Address = user.Address,
                    Country = user.Country,
                    City = user.City,
                    //ProfileImage = user.Picture,
                    //BirthDate = (DateTime)user.BirthDate,
                    ContactNumber = user.ContactNumber

                };
                return View(profile);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Filters.AuthorizationFilter]
        public new ActionResult Profile(ProfileModel profile, HttpPostedFileBase file)
        {
            UserService userService = new UserService();
            var userId = Session["UserId"].ToString();
            User user = userService.GetProfile(userId);
            if (file != null)
            {
                profile.ProfileImage = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), profile.ProfileImage);
                file.SaveAs(path);

                //byte[] bytes;
                //using (BinaryReader br = new BinaryReader(file.InputStream))
                //{
                //    bytes = br.ReadBytes(file.ContentLength);
                //}

                Session["Picture"] = profile.ProfileImage;
                user.Picture = profile.ProfileImage;
            }
            UnitOfWork uow = new UnitOfWork();

            if (user != null)
            {
                if (user.Email != Encryptor.Encrypt(profile.Email))
                {
                    string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
                    user.ValidationToken = Token;
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/emailconfirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
                    user.Status = (int)UserStatus.Pending;
                    uow.UserRepository.SendEmail(profile.Email, "Chill Learn Recover Password", bodyHtml);
                }
                user.Email = Encryptor.Encrypt(profile.Email);
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.Address = profile.Address;
                user.City = profile.City;
                user.Country = profile.Country;
                user.ContactNumber = profile.ContactNumber;
                user.UpdateDate = DateTime.Now;
                uow.Users.Update(user);
                uow.Save();

            }
            return View(profile);
        }

        public ActionResult Classes()
        {
            UnitOfWork uow = new UnitOfWork();
            //List<StudentClasses> sc = uow.StudentRepository.GetClasses(Session["UserId"].ToString(), (int)ClassJoinStatus.Approved);
            List<StudentClasses> sc = uow.StudentRepository.GetClasses(Session["UserId"].ToString());
            StudentClassesViewModel model = new StudentClassesViewModel();
            //model.Pending = sc.Where(e => e.ClassDate > DateTime.Now && (e.RequestStatus == (int)ClassJoinStatus.Pending || e.RequestStatus == (int)ClassJoinStatus.Rejected)).ToList();
            model.Pending = sc.Where(e => (e.RequestStatus == (int)ClassJoinStatus.Pending || e.RequestStatus == (int)ClassJoinStatus.Invited
            || e.RequestStatus == (int)ClassJoinStatus.Rejected)).ToList();

            model.Upcoming = sc.Where(e => e.ClassDate > DateTime.Now &&
            (e.RequestStatus == (int)ClassStatus.Approved || e.ClassStatus == (int)ClassStatus.OfferAccepted)).ToList();

            model.Past = sc.Where(e => e.ClassDate < DateTime.Now && e.RequestStatus != (int)ClassJoinStatus.Rejected).ToList();
            model.Cancelled = sc.Where(e => e.ClassStatus == (int)ClassStatus.Cancelled).ToList();
            //model.Recorded = sc.Where(e => e.ClassDate < DateTime.Now && e.ClassStatus != (int)ClassStatus.Cancelled && e.Record == true).ToList();
            return View(model);
        }

        public ActionResult Wallet()
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                string userid = Session["UserId"].ToString();
                var stuCredits = uow.StudentCredits.Get(a => a.StudentID == userid).FirstOrDefault();
                if (stuCredits != null)
                {
                    ViewBag.TotalBalance = stuCredits.TotalCredits;
                }
                else
                {
                    ViewBag.TotalBalance = 0;
                }
                ViewBag.Subscriptions = uow.Subscriptions.Get(a => a.UserID == userid).ToList();
                ViewBag.CreditLog = GetUserCreditLog(userid); //uow.StudentCreditLogs.Get(a => a.StudentID == userid).ToList();
            }
            catch (Exception)
            {
            }
            return View();
        }

        //temporary method
        public List<StudentCreditLogModel> GetUserCreditLog(string userid)
        {
            using (ChillLearnContext context = new ChillLearnContext())
            {
                var query = from logs in context.StudentCreditLogs
                            join c in context.Classes on logs.ClassID equals c.ClassID
                            where logs.UserID == userid
                            select new StudentCreditLogModel
                            {
                                ClassName = c.Title,
                                CreditsUsed = logs.CreditsUsed,
                                LogType = logs.LogType,
                                CreationDate = logs.CreationDate
                            };
                return query.ToList();
            }
        }

        public ActionResult Notifications()
        {
            string userId = Session["UserId"].ToString();
            UnitOfWork uow = new UnitOfWork();
            List<Notification> notifications = uow.Notifications.Get(x => x.ToUser == userId).OrderByDescending(x => x.CreationDate).ThenByDescending(x => x.IsRead).ToList();
            return View(notifications);
        }

        public ActionResult Teachers(string q, int s = 0)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.SearchTeachers(q, s);
            return View(model);
        }

        public ActionResult booksession(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                UnitOfWork uow = new UnitOfWork();
                ClassViewModel classView = new ClassViewModel();
                List<Subject> subjects = uow.Subjects.Get().ToList();
                classView.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
                classView.SessionTypes = GetSessionTypes();
                classView.TeacherID = id;
                if (!string.IsNullOrEmpty(id))
                {
                    User selectedTeacher = uow.Users.GetByID(id);
                    ViewBag.TeacherName = selectedTeacher.FirstName + " " + selectedTeacher.LastName;
                }
                return View(classView);
            }
            else
                return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult booksession(ClassViewModel model)
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
                    if (model.Record == "1")
                    {
                        record = true;
                    }
                    string userId = Session["UserId"].ToString();
                    string datae = model.Date + " " + model.Time.Insert(model.Time.Length - 2, " ");
                    DateTime combDT = DateTime.ParseExact(datae, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    Class clsCreate = new Class()
                    {
                        ClassID = Guid.NewGuid().ToString(),
                        Title = model.Title,
                        ClassDate = combDT,
                        StartTime = model.Time,
                        Duration = model.Duration,
                        CreationDate = DateTime.Now,
                        Type = model.SessionType,
                        Record = record,
                        CreatedBy = userId,
                        TeacherID = model.TeacherID,
                        Description = model.Description,
                        SubjectID = model.Subject,
                        Status = (int)ClassStatus.Requested,
                        BrainCertId = 0, //model.BrainCertId,
                        CreatedByStudent = true
                    };
                    uow.Classes.Insert(clsCreate);
                    uow.Save();
                    AddClassFiles(model.files, clsCreate.ClassID);
                    //add notification for teacher
                    Common.AddNotification(Session["UserName"] + " requested a session for " + model.Duration + " hours", "", userId, model.TeacherID, "/tutor/classdetail/" + clsCreate.ClassID, (int)NotificationType.Class);
                    //
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
            catch (Exception) { }
        }

        [HttpGet]
        public ActionResult LaunchClass(string classId)
        {
            int brainCertClassId = Convert.ToInt32(Encryptor.Decrypt(classId));
            string userId = Session["UserId"].ToString();
            UnitOfWork uow = new UnitOfWork();
            ClassDetail classDetail = uow.StudentRepository.GetClassDetail(brainCertClassId);
            User currentUser = uow.Users.GetByID(userId);
            BrainCert bc = new BrainCert();
            string launchUrl = bc.GetLaunchURL(brainCertClassId, currentUser.AutoID, currentUser.FirstName + " " + currentUser.FirstName, classDetail.Title, classDetail.SubjectName, (currentUser.UserRole == (int)UserRoles.Teacher));
            return Redirect(launchUrl);
        }
    }
}