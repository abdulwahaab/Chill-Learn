using System;
using System.Web;
using System.Web.Mvc;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChillLearn.ViewModels
{
    public class ClassModel
    {
    }

    public class ClassViewModel
    {
        public string ClassID { get; set; }
        public string TeacherID { get; set; }

        [Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Select Date")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Please Select Time")]
        public string Time { get; set; }

        [Required(ErrorMessage = "Please Select Session")]
        public int SessionType { get; set; }

        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Enter Duration")]
        public decimal Duration { get; set; }

        public string Record { get; set; }

        [Required(ErrorMessage = "Please Select Subject")]
        public int Subject { get; set; }

        public int? BrainCertId { get; set; }

        public List<SelectListItem> SessionTypes { get; set; }

        public SelectList Subjects { get; set; }

        public HttpPostedFileBase[] files { get; set; }
    }

    public class ClassFindParam
    {
        public SelectList Teachers { get; set; }
        public SelectList Subjects { get; set; }
        public List<SelectListItem> SessionTypes { get; set; }
        public SearchParam Search { get; set; }
        public List<SearchClassModel> Classes { get; set; }

    }
    public class SearchParam
    {
        public string TeacherId { get; set; }
        public string q { get; set; }
        public int SubjectId { get; set; }
        public int SessionType { get; set; }
    }

    public class ClassActionParam
    {
        public string ClassId { get; set; }
    }

    public class StudentClassesViewModel
    {
        public List<StudentClasses> Upcoming { get; set; }
        public List<StudentClasses> Pending { get; set; }
        public List<StudentClasses> Past { get; set; }
        public List<StudentClasses> Cancelled { get; set; }
        public List<StudentClasses> Recorded { get; set; }
    }

    public class StudentClassUpdateParam
    {
        public string StudentClassId { get; set; }
        public string StudentId { get; set; }
        public string ClassId { get; set; }
        public string Status { get; set; }
    }


}