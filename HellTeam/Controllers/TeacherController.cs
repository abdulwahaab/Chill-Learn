﻿using System.Linq;
using ChillLearn.DAL;
using System.Web.Mvc;
using ChillLearn.CustomModels;

namespace ChillLearn.Controllers
{
    public class TeacherController : BaseController
    {
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