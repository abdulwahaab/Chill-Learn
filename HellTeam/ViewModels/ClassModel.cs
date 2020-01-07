using System.Web;
using System.Web.Mvc;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using ExpressiveAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChillLearn.ViewModels
{
    public class ClassViewModel
    {
        public string ClassID { get; set; }
        public string TeacherID { get; set; }

        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Please provide class title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select date")]
        public string Date { get; set; }

        public string StartTime { get; set; }

        public string ClassEndTime { get; set; }

        [Range(1, 2, ErrorMessage = "Please select class type")]
        public int SessionType { get; set; }

        [MaxLength(1500, ErrorMessage = "Description must be less than 1500 characters")]
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }

        public decimal? Duration { get; set; }

        public string Record { get; set; }

        [Required(ErrorMessage = "Please select subject")]
        public int Subject { get; set; }

        public int? BrainCertId { get; set; }

        [RequiredIf("SessionType == 1", ErrorMessage = "Please select a time zone")]
        public int? TimeZone { get; set; }

        [RequiredIf("SessionType == 1", ErrorMessage = "Please select class time")]
        public string ClassHour { get; set; }

        [RequiredIf("SessionType == 1")]
        public string ClassMinute { get; set; }

        [RequiredIf("SessionType == 1")]
        public string ClassAMPM { get; set; }

        public string DurationHour { get; set; }

        public string DurationMinutes { get; set; }

        public List<SelectListItem> SessionTypes { get; set; }

        public SelectList Subjects { get; set; }

        public SelectList TimeZones { get; set; }

        public SelectList HourList { get; set; }

        public SelectList MinuteList { get; set; }

        public SelectList AMPMList { get; set; }

        public SelectList DurationHourList { get; set; }

        public SelectList DurationMinuteList { get; set; }

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