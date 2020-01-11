using System;
using System.IO;
using PayPal.Api;
using System.Web;
using System.Linq;
using ChillLearn.DAL;
using System.Web.Mvc;
using System.Globalization;
using ChillLearn.ViewModels;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;

namespace ChillLearn.Controllers
{
    [Filters.AuthorizeStudent]
    public class StudentController : BaseController
    {
        private PayPal.Api.Payment payment;

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
            ProblemsModel model = new ProblemsModel();
            try
            {
                UnitOfWork uow = new UnitOfWork();
                List<TeacherSubject> subjects = uow.TeacherRepository.GetSubjects(id);
                model.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
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
            catch (Exception)
            {
                ModelState.AddModelError("error", Resources.Resources.MsgErrorTryAgain);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateProblem(ProblemsModel model, List<HttpPostedFileBase> files)
        {
            UnitOfWork uow = new UnitOfWork();
            List<TeacherSubject> subjects = uow.TeacherRepository.GetSubjects(model.TeacherID);
            model.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypess();
            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.TeacherID))
                {
                    User selectedTeacher = uow.Users.GetByID(model.TeacherID);
                    ViewBag.TeacherName = selectedTeacher.FirstName + " " + selectedTeacher.LastName;
                }
                ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
                return View(model);
            }
            string userId = Session["UserId"].ToString();
            if (Common.UserHasCredits(userId, model.HoursNeeded))
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
                    ContactNumber = user.ContactNumber,
                    FullPhone = user.ContactNumber

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
                user.ContactNumber = profile.FullPhone;
                user.UpdateDate = DateTime.Now;
                uow.Users.Update(user);
                uow.Save();

            }
            //profile.ContactNumber = user.ContactNumber;
            //return View(profile);
            return RedirectToAction("profile", "student");
        }

        public ActionResult Classes()
        {
            UnitOfWork uow = new UnitOfWork();
            //List<StudentClasses> sc = uow.StudentRepository.GetClasses(Session["UserId"].ToString(), (int)ClassJoinStatus.Approved);
            List<StudentClasses> sc = uow.StudentRepository.GetClasses(Session["UserId"].ToString());
            StudentClassesViewModel model = new StudentClassesViewModel();
            //model.Pending = sc.Where(e => e.ClassDate > DateTime.Now && (e.RequestStatus == (int)ClassJoinStatus.Pending || e.RequestStatus == (int)ClassJoinStatus.Rejected)).ToList();
            model.Pending = sc.Where(e => (e.RequestStatus == (int)ClassJoinStatus.Pending || e.RequestStatus == (int)ClassJoinStatus.Invited || e.RequestStatus == (int)ClassJoinStatus.Rejected)).ToList();
            model.Upcoming = sc.Where(e => e.ClassDate > DateTime.Now && (e.RequestStatus == (int)ClassStatus.Approved || e.ClassStatus == (int)ClassStatus.OfferAccepted)).ToList();
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
            List<ChillLearn.Data.Models.Notification> notifications = uow.Notifications.Get(x => x.ToUser == userId).OrderByDescending(x => x.CreationDate).ThenByDescending(x => x.IsRead).ToList();
            return View(notifications);
        }

        public ActionResult Teachers(string q, int s = 0)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.SearchTeachers(q, s);
            return View(model);
        }

        public ActionResult BookSession(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                UnitOfWork uow = new UnitOfWork();
                ClassViewModel model = new ClassViewModel();
                //List<TeacherSubject> subjects = uow.TeacherRepository.GetSubjects(id);
                //model.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
                //model.SessionTypes = GetSessionTypes();
                model.TeacherID = id;
                if (!string.IsNullOrEmpty(id))
                {
                    User selectedTeacher = uow.Users.GetByID(id);
                    model.TeacherName = selectedTeacher.FirstName + " " + selectedTeacher.LastName;
                }
                return View(ReturnCreateClassView(model, model.TeacherID));
            }
            else
                return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult BookSession(ClassViewModel model)
        {
            try
            {
                string userId = Session["UserId"].ToString();
                decimal classDuration = Math.Round(Convert.ToInt16(model.DurationHour) + (Convert.ToInt16(model.DurationMinutes) / 60m), 2);
                if (Common.UserHasCredits(userId, classDuration))
                {
                    UnitOfWork uow = new UnitOfWork();
                    if (Convert.ToInt32(model.DurationHour) < 1 && Convert.ToInt32(model.DurationMinutes) < 30)
                    {
                        ModelState.AddModelError("classtime-error", Resources.Resources.MsgClassDurationError);
                        return View(ReturnCreateClassView(model, model.TeacherID));
                    }
                    else if (!ModelState.IsValid)
                    {
                        return View(ReturnCreateClassView(model, model.TeacherID));
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
                            dateAndTime = model.Date + " " + model.ClassHour + ":" + model.ClassMinute + " " + model.ClassAMPM;
                            classDate = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        if (model.SessionType == 1)
                        {
                            string classEndTime = classDate.AddHours(Convert.ToDouble(model.DurationHour)).AddMinutes(Convert.ToDouble(model.DurationMinutes)).ToString("HH:mm tt");
                            model.StartTime = classDate.ToString("hh:mm tt");
                            // only 30 minute class is allowed in trial period
                            model.ClassEndTime = classEndTime; //classDate.AddMinutes(30).ToString("hh:mm tt");
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
                            CreatedBy = userId,
                            TeacherID = model.TeacherID,
                            Description = model.Description,
                            SubjectID = model.Subject,
                            Status = (int)ClassStatus.Requested,
                            BrainCertId = 0,
                            TimeZone = model.TimeZone,
                            CreatedByStudent = true
                        };
                        uow.Classes.Insert(clsCreate);
                        uow.Save();
                        AddClassFiles(model.files, clsCreate.ClassID);
                        //add notification for teacher
                        Common.AddNotification(Session["UserName"] + " requested a session", "", userId, model.TeacherID, "/tutor/classdetail/" + clsCreate.ClassID, (int)NotificationType.Class);
                        //Common.AddNotification(Session["UserName"] + " requested a session for " + model.Duration + " hours", "", userId, model.TeacherID, "/tutor/classdetail/" + clsCreate.ClassID, (int)NotificationType.Class);
                        //
                        string teacherId = model.TeacherID;
                        ClassViewModel classView = new ClassViewModel();
                        ModelState.Clear();
                        ModelState.AddModelError("success", Resources.Resources.MsgClassCreatedSuccess);
                        return View(ReturnCreateClassView(classView, teacherId));
                    }
                }
                else
                {
                    ModelState.AddModelError("error", Resources.Resources.MsgNoBalance);
                    return View(ReturnCreateClassView(model, model.TeacherID));
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ClassViewModel ReturnCreateClassView(ClassViewModel model, string teacherId)
        {
            Common common = new Common();
            UnitOfWork uow = new UnitOfWork();
            if (!string.IsNullOrEmpty(teacherId))
            {
                User selectedTeacher = uow.Users.GetByID(teacherId);
                model.TeacherName = selectedTeacher.FirstName + " " + selectedTeacher.LastName;
            }
            model.SessionType = 1;
            model.DurationHourList = new SelectList(common.GetDurationHours());
            model.DurationMinuteList = new SelectList(common.GetMinutes());
            model.HourList = new SelectList(common.GetHours());
            model.MinuteList = new SelectList(common.GetMinutes());
            model.AMPMList = new SelectList(common.GetAMPM());
            model.TimeZones = new SelectList(uow.TimeZones.Get(), "GMT", "Name");
            model.Subjects = new SelectList(uow.TeacherRepository.GetSubjects(teacherId), "SubjectID", "SubjectName");
            model.SessionTypes = GetSessionTypes();
            return model;
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
            string launchUrl = bc.GetLaunchURL(brainCertClassId, currentUser.AutoID, currentUser.FirstName + " " + currentUser.LastName, classDetail.Title, classDetail.SubjectName, (currentUser.UserRole == (int)UserRoles.Teacher));
            return Redirect(launchUrl);
        }

        [HttpGet]
        public ActionResult Plans()
        {
            UnitOfWork uow = new UnitOfWork();
            List<Data.Models.Plan> plans = uow.Plans.Get(a => a.Status == 1).ToList();
            return View(plans);
        }

        [HttpPost]
        public ActionResult Plans(Data.Models.Plan model)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    UnitOfWork uow = new UnitOfWork();
                    Data.Models.Plan plan = uow.Plans.Get(a => a.PlanID == model.PlanID).FirstOrDefault();
                    APIContext apiContext = PaypalConfiguration.GetAPIContext();
                    string payerId = Request.Params["PayerID"];
                    var guid = Convert.ToString(new Random().Next(100000));
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/student/paymentcomplete?pd=" + plan.PlanID + "&guid=" + guid;
                    var createdPayment = CreatePayment(apiContext, baseURI, plan);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;
                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                            paypalRedirectUrl = link.href;
                    }
                    Session.Add(guid, createdPayment.id);
                    Session["guid"] = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("error", Resources.Resources.MsgErrorTryAgain);
                    return View();
                }
            }
            else
            {
                return RedirectToAction("login", "account");
            }
        }

        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectUrl, Data.Models.Plan plan)
        {
            var listItems = new ItemList() { items = new List<Item>() };
            listItems.items.Add(new Item() { name = plan.PlanName, currency = "USD", price = plan.Price.ToString(), quantity = "1" });
            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls() { cancel_url = redirectUrl, return_url = redirectUrl };
            var details = new Details() { tax = "0", shipping = "0", subtotal = plan.Price.ToString() };
            var amount = new Amount() { currency = "USD", total = plan.Price.ToString(), details = details };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = plan.PlanName + " Plan Payment Description",
                invoice_number = Convert.ToString(new Random().Next(100000)),
                amount = amount,
                item_list = listItems
            });
            payment = new PayPal.Api.Payment() { intent = "sale", payer = payer, transactions = transactionList, redirect_urls = redirUrls };
            return payment.Create(apiContext);
        }

        public ActionResult paymentcomplete()
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    return RedirectToAction("plans");
                }
                else
                {
                    PayPal.Api.Payment executePayment = ExecutePayment(apiContext, payerId, Session["guid"] as string);
                    if (executePayment.state.ToLower() == "approved")
                    {
                        string planId = Request.Params["pd"];
                        string token = Request.Params["token"];
                        UnitOfWork uow = new UnitOfWork();
                        string userId = Session["UserId"] as string;
                        Data.Models.Plan plan = uow.Plans.Get(a => a.PlanID == planId).FirstOrDefault();
                        AddStudentCredits(userId, plan);
                        string subId = AddSubscriptions(userId, plan);
                        AddPayment(userId, plan, subId, payerId, Session["guid"] as string, token);
                        ModelState.AddModelError("success", Resources.Resources.TxtSuccessfullyPurchased);
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("error", Resources.Resources.TxtFailedPurchased + Resources.Resources.TxtContactSupport);
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("error", Resources.Resources.MsgErrorTryAgain);
                return View();
            }
        }

        private PayPal.Api.Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            try
            {
                var paymentExecution = new PaymentExecution() { payer_id = payerId };
                payment = new PayPal.Api.Payment() { id = paymentId };
                return payment.Execute(apiContext, paymentExecution);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddStudentCredits(string studentId, Data.Models.Plan plan)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                StudentCredit studentCredit = uow.StudentCredits.Get(a => a.StudentID == studentId).FirstOrDefault();
                if (studentCredit != null)
                {
                    studentCredit.TotalCredits = studentCredit.TotalCredits + plan.Hours;
                    studentCredit.LastUpdates = DateTime.Now;
                    uow.StudentCredits.Update(studentCredit);
                }
                else
                {
                    StudentCredit credit = new StudentCredit
                    {
                        StudentID = studentId,
                        LastUpdates = DateTime.Now,
                        TotalCredits = plan.Hours,
                        UsedCredits = 0
                    };
                    uow.StudentCredits.Insert(credit);
                }
                uow.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AddSubscriptions(string studentId, Data.Models.Plan plan)
        {
            try
            {
                Subscription subscription = new Subscription()
                {
                    SubscriptionID = Guid.NewGuid().ToString(),
                    PlanID = plan.PlanID,
                    Hours = plan.Hours,
                    UserID = studentId,
                    CreationDate = DateTime.Now,
                    Status = 1
                };
                UnitOfWork uow = new UnitOfWork();
                uow.Subscriptions.Insert(subscription);
                uow.Save();
                return subscription.SubscriptionID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPayment(string userId, Data.Models.Plan plan, string subId, string payerId, string paymentId, string token)
        {
            try
            {
                Data.Models.Payment payment1 = new Data.Models.Payment()
                {
                    UserID = userId,
                    SubscriptionID = subId,
                    Type = 1,
                    Amount = plan.Price,
                    Source = (int)PaymentSource.Paypal,
                    CreationDate = DateTime.Now,
                    Status = 1,
                    PaypalToken = token,
                    PayerId = payerId,
                    PayPaymentId = paymentId
                };
                UnitOfWork uow = new UnitOfWork();
                uow.Payments.Insert(payment1);
                uow.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}