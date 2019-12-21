using ChillLearn.CustomModels;
using ChillLearn.DAL;
using ChillLearn.DAL.Services;
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
    [Filters.AuthorizeStudent]
    public class TeachersController : BaseController
    {
        public ActionResult index(string q, int s = 0)
        {
            ViewBag.Keyword = q;
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.SearchTeachers(q, s);
            ViewBag.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            return View(model);
        }
    }
}