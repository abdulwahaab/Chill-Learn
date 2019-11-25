using ChillLearn.CustomModels;
using ChillLearn.DAL;
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
    [Filters.AuthorizeTeacher]
    public class TutorController : Controller
    {
        // GET: Tutor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Applications()
        {
            return View();
        }

        public ActionResult Payment_Roll()
        {
            return View();
        }
        public ActionResult Payment_History()
        {
            return View();
        }

        public ActionResult StudentProblems()
        {
            UnitOfWork uow = new UnitOfWork();
            List<StudentProblemsModel> problems = uow.UserRepository.GetProblems(Session["UserId"].ToString());
            return View(problems);
        }
        public ActionResult WriteProposal(string q)
        {
            if (q != null)
            {
                UnitOfWork uow = new UnitOfWork();
                QuestionDetailModel model = new QuestionDetailModel();
                model.QuestionDetail = uow.UserRepository.GetQuestionDetailById(q);
                if (model.QuestionDetail != null)
                {
                    model.ProblemId = q;
                    return View(model);
                }
                return RedirectToAction("studentproblems", "tutor");
            }
            else
            {
                return RedirectToAction("studentproblems", "tutor");
            }
        }
        [HttpPost]
        public ActionResult WriteProposal(QuestionDetailModel model)
        {
            UnitOfWork uow = new UnitOfWork();
            model.QuestionDetail = uow.UserRepository.GetQuestionDetailById(model.ProblemId);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Please provide valid information.");
                return View(model);
            }
            StudentProblemBid problem = new StudentProblemBid
            {
                BidID = Guid.NewGuid().ToString(),
                UserID = Session["UserId"].ToString(),
                ProblemID = model.ProblemId,
                CreationDate = DateTime.Now,
                Description = model.Response,
                Status = (int)BidStatus.Pending,
            };
            uow.StudentProblemBids.Insert(problem);
            uow.Save();
            ModelState.AddModelError("success", "Proposal submited successfully.");
            return View(model);
        }

        public ActionResult Bids()
        {
            UnitOfWork uow = new UnitOfWork();
            List<BidsModel> model = uow.UserRepository.GetBidsByUserId(Session["UserId"].ToString());
            return View(model);
        }

        public ActionResult Profile()
        {
            UserService userService = new UserService();
            UnitOfWork uow = new UnitOfWork();
            var userId = Session["UserId"].ToString();
            TeacherProfileModel teacherProfile = uow.TeacherRepository.GetTeacherProfile(userId.ToString());
            ViewBag.TeacherStages = uow.TeacherRepository.GetTeacherStages(userId.ToString());
            ViewBag.TeacherQualifications = uow.TeacherQualifications.Get().Where(a => a.TeacherID == userId).ToList();
            ViewBag.TeacherCertification = uow.TeacherCertifications.Get().Where(a => a.TeacherId == userId).ToList();
            ViewBag.Stages = uow.Stages.Get().ToList();
            if (teacherProfile != null)
            {
                teacherProfile.Email = Encryptor.Decrypt(teacherProfile.Email);
                return View(teacherProfile);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Filters.AuthorizationFilter]
        public ActionResult Profile(TeacherProfileModel profile, HttpPostedFileBase file)
        {
            UnitOfWork uow = new UnitOfWork();
            UserService userService = new UserService();
            var userId = Session["UserId"].ToString();
            ViewBag.TeacherStages = uow.TeacherRepository.GetTeacherStages(userId.ToString());
            ViewBag.TeacherQualifications = uow.TeacherQualifications.Get().Where(a => a.TeacherID == userId).ToList();
            ViewBag.TeacherCertification = uow.TeacherCertifications.Get().Where(a => a.TeacherId == userId).ToList();
            ViewBag.Stages = uow.Stages.Get().ToList();
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
                    string bodyHtml = "<p>Welcome to Chill Learn</p> <p> please <a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>Click Here</a> to confirm email </p>";
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

                TeacherDetail teacherDetail = uow.TeacherDetails.Get().Where(a => a.TeacherID == user.UserID).FirstOrDefault();
                if(teacherDetail != null)
                {
                    teacherDetail.Title = profile.Title;
                    teacherDetail.Qualification = profile.Qualification;
                    teacherDetail.YearsExperience = profile.Experience;
                    teacherDetail.Description = profile.Description;
                    uow.TeacherDetails.Update(teacherDetail);
                }
                else
                {
                    TeacherDetail teacher = new TeacherDetail
                    {
                        Title = profile.Title,
                        Description = profile.Description,
                        CreationDate = DateTime.Now,
                        Qualification = profile.Qualification,
                        Status = 1,
                        YearsExperience = profile.Experience,
                        TeacherID = userId
                    };
                    uow.TeacherDetails.Insert(teacher);
                }

                uow.Save();
            }
            return View(profile);
        }
        public ActionResult Classes()
        {
            UnitOfWork uow = new UnitOfWork();
            List<ClassesModel> model = uow.TeacherRepository.GetClasses(Session["UserId"].ToString());
            return View(model);
        }
        public ActionResult Requests(string c)
        {
            if (c != null)
            {
                UnitOfWork uow = new UnitOfWork();
                List<RequestsModel> model = uow.TeacherRepository.GetClassRequests(c);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public bool UpdateClassStatus(StudentClassUpdateParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                StudentClass studentClasses = uow.StudentClasses.GetByID(Convert.ToInt32(model.StudentClassId));
                if (studentClasses != null)
                {
                    studentClasses.Status = (int)ClassJoinStatus.Rejected;
                    studentClasses.JoiningDate = DateTime.Now;
                    if (model.Status == "accept")
                    {
                        studentClasses.Status = (int)ClassJoinStatus.Approved;
                    }
                    uow.StudentClasses.Update(studentClasses);
                    uow.Save();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
      
        }

        [HttpGet]
        public JsonResult GetSubjects(string name)
        {
            UnitOfWork uow = new UnitOfWork();
            var list = uow.Subjects.Get().Where(x => x.SubjectName.ToLower().Contains(name.ToLower())).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public bool AddTeacherStage(TeacherStageParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                Subject subject = uow.Subjects.Get().Where(a => a.SubjectName == model.SubjectName).FirstOrDefault();
                if (subject == null)
                {
                    Subject subject1 = new Subject();
                    subject1.SubjectName = model.SubjectName;
                    subject1.Description = "test";
                    subject1.Status = 1;
                    uow.Subjects.Insert(subject1);
                    uow.Save();
                }
                Subject subject2 = uow.Subjects.Get().Where(a => a.SubjectName == model.SubjectName).FirstOrDefault();
                var userId = Session["UserId"].ToString();
                TeacherStage teacherStage = new TeacherStage();
                teacherStage.StageID = model.StageId;
                teacherStage.SubjectID = subject2.SubjectID;
                teacherStage.TeacherID = userId;
                //teacherStage.HourlyRate = model.HourlyRate;

                uow.TeacherStages.Insert(teacherStage);
                uow.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        [HttpPost]
        public bool AddTeacherQualification(QualificationParam model)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                var userId = Session["UserId"].ToString();
                TeacherQualification td = new TeacherQualification();
                td.DegreeTitle = model.DegreeTitle;
                td.InstituteName = model.InstituteName;
                td.TeacherID = userId;
                td.YearPassed = model.YearPassed;
                td.CreationDate = DateTime.Now;
                td.Status = 1;
                uow.TeacherQualifications.Insert(td);
                uow.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        [HttpPost]
        public bool AddTeacherCertification(QualificationParam model)
        {
            try
            {
                string imageName = "";
                TeacherCertification td = new TeacherCertification();
                var file = Request.Files[0];
                if (file != null)
                {
                    imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/images/certificates/"), imageName);
                    file.SaveAs(path);
                    td.Image = imageName;
                }
                UnitOfWork uow = new UnitOfWork();
                var userId = Session["UserId"].ToString();
                td.Title = model.DegreeTitle;
                td.Institute = model.InstituteName;
                td.TeacherId = userId;
                td.Year = model.YearPassed;
                td.CreationDate = DateTime.Now;
                td.IsActive = true;
                uow.TeacherCertifications.Insert(td);
                uow.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}