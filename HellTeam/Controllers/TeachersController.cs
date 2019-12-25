using System.Web.Mvc;
using ChillLearn.DAL;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using System.Linq;

namespace ChillLearn.Controllers
{
    public class TeachersController : BaseController
    {
        [Filters.AuthorizeStudent]
        public ActionResult index(string q, int s = 0)
        {
            ViewBag.Keyword = q;
            UnitOfWork uow = new UnitOfWork();
            List<SearchModel> model = uow.UserRepository.SearchTeachers(q, s);
            ViewBag.Subjects = new SelectList(uow.Subjects.Get(), "SubjectID", "SubjectName");
            return View(model);
        }

        public new ActionResult Profile(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            TeacherProfileView profileView = new TeacherProfileView();
            if (!string.IsNullOrEmpty(id))
            {
                profileView.Profile = uow.TeacherRepository.GetTeacherProfile(id);
                string email = Encryptor.Decrypt(profileView.Profile.Email);
                profileView.Profile.Email = email;
                profileView.Subjects = uow.TeacherRepository.GetTeacherStages(id);
                profileView.Education = uow.TeacherQualifications.Get(a => a.TeacherID == id).ToList();
            }
            return View(profileView);
        }
    }
}