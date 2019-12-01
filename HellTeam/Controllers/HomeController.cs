using ChillLearn.DAL;
using ChillLearn.CustomModels;
using ChillLearn.DAL.Services;
using ChillLearn.Data.Models;
using ChillLearn.Enums;
using ChillLearn.ViewModels;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using PayPal.Api;

namespace ChillLearn.Controllers
{
    public class HomeController : BaseController
    {
        private PayPal.Api.Payment payment;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserView userView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", Resources.Resources.InvalidInfo);
                return View(userView);
            }
            UnitOfWork uow = new UnitOfWork();
            string encryptedEmail = Encryptor.Encrypt(userView.Email);
            string encryptedPassword = Encryptor.Encrypt(userView.Password);
            string Token = Encryptor.Encrypt(DateTime.Now.Ticks.ToString());
            UserService us = new UserService();
            if (!us.DoesEmailExist(encryptedEmail))
            {
                if (!us.DoesContactNoExist(userView.ContactNumber))
                {
                    User user = new User()
                    {
                        UserID = Guid.NewGuid().ToString(),
                        FirstName = userView.FirstName,
                        LastName = userView.LastName,
                        Class = userView.Class,
                        ContactNumber = userView.ContactNumber,
                        CreationDate = DateTime.Now,
                        Email = encryptedEmail,
                        Grade = userView.Grade,
                        Password = encryptedPassword,
                        UserRole = userView.UserRole,
                        Status = (int)UserStatus.Pending,
                        ValidationToken = Token,
                        Source = (int)SignupSource.App
                    };
                    uow.Users.Insert(user);
                    uow.Save();
                    var scheme = Request.Url.Scheme + "://";
                    var host = Request.Url.Host + ":";
                    var port = Request.Url.Port;
                    string host1 = scheme + host + port;
                    string activationLink = "<a href='" + host1 + "/account/email_confirmation?token=" + Token + "'>" + Resources.Resources.ClickHere + "</a>";
                    Utility.SendAccountActivationEmail(userView.Email, userView.FirstName, activationLink);
                    ViewBag.Message = Resources.Resources.AccountSuccess;
                    return View(userView);
                }
                else
                {
                    ModelState.AddModelError("error", Resources.Resources.PhoneExists);
                }
            }
            else
            {
                ModelState.AddModelError("error", Resources.Resources.EmailAlreadyExists);
            }
            return View(userView);
        }

        public ActionResult main()
        {
            return View();
        }

        [Filters.AuthorizationFilter]
        public ActionResult search(int p)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.GetTutorBySubject(p);
            return View(model);
        }

        [Filters.AuthorizationFilter]
        public ActionResult Profile(string p)
        {
            UnitOfWork uow = new UnitOfWork();
            TeacherProfileView profileView = new TeacherProfileView();
            if (!string.IsNullOrEmpty(p))
            {
                profileView.Profile = uow.TeacherRepository.GetTeacherProfile(p);
                string email = Encryptor.Decrypt(profileView.Profile.Email);
                profileView.Profile.Email = email;
                profileView.Subjects = uow.TeacherRepository.GetTeacherStages(p);
                profileView.Education = uow.TeacherQualifications.Get().Where(a => a.TeacherID == p).ToList();
            }
            return View(profileView);
        }

        [HttpGet]
        public JsonResult GetSubjects(string name)
        {
            UnitOfWork uow = new UnitOfWork();
            var list = uow.Subjects.Get().Where(x => x.SubjectName.ToLower().Contains(name.ToLower())).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChnageLang(string lang)
        {
            Session["lang"] = lang;
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pricing()
        {
            UnitOfWork uow = new UnitOfWork();
            List<Data.Models.Plan> plans = uow.Plans.Get().ToList();
            return View(plans);
        }

        [Filters.AuthorizationFilter]
        public ActionResult PlanDetail(string p)
        {
            UnitOfWork uow = new UnitOfWork();
            Data.Models.Plan plan = uow.Plans.Get().Where(a => a.PlanID == p).FirstOrDefault();
            return View(plan);
        }

        [HttpPost]
        [Filters.AuthorizationFilter]
        public ActionResult PlanDetail(Data.Models.Plan model)
        {
            UnitOfWork uow = new UnitOfWork();
            Data.Models.Plan plan = uow.Plans.Get().Where(a => a.PlanID == model.PlanID).FirstOrDefault();
            return BuyNow(plan);
        }

        [HttpPost]
        [Filters.AuthorizationFilter]
        public ActionResult BuyNow(Data.Models.Plan plan)
        {
            if (Session["UserId"] != null)
            {
                APIContext apiContext = PaypalConfiguration.GetAPIContext();
                try
                {
                    string payerId = Request.Params["PayerID"];
                    if (string.IsNullOrEmpty(payerId))
                    {
                        string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/home/paypaldetail?pd=" + plan.PlanID + "&";
                        var guid = Convert.ToString(new Random().Next(100000));
                        var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, plan);

                        var links = createdPayment.links.GetEnumerator();
                        string paypalRedirectUrl = string.Empty;
                        while (links.MoveNext())
                        {
                            Links link = links.Current;
                            if (link.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                paypalRedirectUrl = link.href;
                            }
                        }
                        Session.Add(guid, createdPayment.id);
                        Session["guid"] = createdPayment.id;
                        return Redirect(paypalRedirectUrl);
                    }
                    else
                    {
                        //var guid = Request.Params["guid"];
                        PayPal.Api.Payment executePayment = ExecutePayment(apiContext, payerId, Session["guid"] as string);
                        if (executePayment.state.ToLower() != "approved")
                        {
                            return View("Failure");
                        }
                        else
                        {
                            string planId = Request.Params["pd"];
                            string token = Request.Params["token"];
                            AddPaymentDetail(planId, payerId, Session["guid"] as string, token);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View("Failure");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { plan_id = plan.PlanID });
            }
            return View();
        }

        [Filters.AuthorizationFilter]
        public ActionResult PaypalDetail(Data.Models.Plan plan)
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/home/paypaldetail?pd=" + plan.PlanID + "&";
                    var guid = Convert.ToString(new Random().Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, plan);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;
                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    Session["guid"] = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    //var guid = Request.Params["guid"];
                    PayPal.Api.Payment executePayment = ExecutePayment(apiContext, payerId, Session["guid"] as string);
                    if (executePayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                    else
                    {
                        string planId = Request.Params["pd"];
                        string token = Request.Params["token"];
                        AddPaymentDetail(planId, payerId, Session["guid"] as string, token);
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Failure");
            }
            return View("Success");
        }

        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectUrl, Data.Models.Plan plan)
        {
            //var plan = new Data.Models.Plan();
            var listItems = new ItemList() { items = new List<Item>() };
            listItems.items.Add(new Item()
            {
                name = plan.PlanName,
                currency = "USD",
                price = plan.Price.ToString(),
                quantity = "1",
            });
            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = plan.Price.ToString()
            };
            var amount = new Amount()
            {
                currency = "USD",
                total = plan.Price.ToString(),
                details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = plan.PlanName + " Plan Payment Description",
                invoice_number = Convert.ToString(new Random().Next(100000)),
                amount = amount,
                item_list = listItems,

            });
            payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return payment.Create(apiContext);
        }

        private PayPal.Api.Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            try
            {
                var paymentExecution = new PaymentExecution()
                {
                    payer_id = payerId
                };
                payment = new PayPal.Api.Payment()
                {
                    id = paymentId
                };
                return payment.Execute(apiContext, paymentExecution);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddPaymentDetail(string planId, string payerId, string paymentId, string token)
        {
            UnitOfWork uow = new UnitOfWork();
            string userId = Session["UserId"] as string;
            Data.Models.Plan plan = uow.Plans.Get().Where(a => a.PlanID == planId).FirstOrDefault();
            AddStudentCredits(userId, plan);
            string subId = AddSubscriptions(userId, plan);
            AddPayment(userId, plan, subId, payerId, paymentId, token);
        }

        public void AddStudentCredits(string studentId, Data.Models.Plan plan)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork();
                StudentCredit studentCredit = uow.StudentCredits.Get().Where(a => a.StudentID == studentId).FirstOrDefault();
                if (studentCredit != null)
                {
                    studentCredit.TotalCredits = studentCredit.TotalCredits + plan.Credits;
                    studentCredit.LastUpdates = DateTime.Now;
                    uow.StudentCredits.Update(studentCredit);
                }
                else
                {
                    StudentCredit credit = new StudentCredit
                    {
                        StudentID = studentId,
                        LastUpdates = DateTime.Now,
                        TotalCredits = plan.Credits,
                        UsedCredits = 0
                    };
                    uow.StudentCredits.Insert(credit);
                }
                uow.Save();
            }
            catch (Exception ex)
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
                    Credits = plan.Credits,
                    UserID = studentId,
                    CreationDate = DateTime.Now,
                    Status = 1
                };
                UnitOfWork uow = new UnitOfWork();
                uow.Subscriptions.Insert(subscription);
                uow.Save();
                return subscription.SubscriptionID;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}