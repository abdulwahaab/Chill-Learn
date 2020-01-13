using System.Web;
using System.Web.Mvc;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using ExpressiveAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChillLearn.ViewModels
{
    public partial class ProblemsModel
    {
        [Display(Name = "Select Type")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgSelectType")]
        public int? Type { get; set; }

        [Display(Name = "Select Subject")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgSelectSubject")]
        public int Subject { get; set; }

        [Display(Name = "Hours Needed")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgHoursNeeded")]
        public decimal HoursNeeded { get; set; }

        [Display(Name = "Set Deadline")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgDeadline")]
        public string DeadLine { get; set; }

        //[Display(Name = "Describe your problem")]
        //[Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgProvideDescription")]
        public string TeacherID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgProvideDescription")]
        public string ProblemDescription { get; set; }

        public string DurationHour { get; set; }

        public string DurationMinutes { get; set; }

        public List<SelectListItem> SessionTypes { get; set; }

        public SelectList Subjects { get; set; }

        public List<StudentProblemsModel> Problems { get; set; }

        public SelectList DurationHourList { get; set; }

        public SelectList DurationMinuteList { get; set; }
    }

    public partial class ProblemDetailModel
    {
        [Display(Name = "Response")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgProvideDescription")]
        public string Response { get; set; }

        [Required]
        public string ProblemId { get; set; }

        public QuestionModel ProblemDetails { get; set; }
    }

    public partial class QuestionDetailModel
    {
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgProvideProposalDescription")]
        public string Response { get; set; }

        [Required]
        public string ProblemId { get; set; }

        public QuestionModel QuestionDetail { get; set; }
    }

    public class ProposalDetailModel
    {
        [Display(Name = "Response")]
        //[Required(ErrorMessage = "Enter message")]
        public string Response { get; set; }

        //[Required]
        public string BidId { get; set; }

        public string ProblemID { get; set; }

        public string ToUser { get; set; }

        public string FromUser { get; set; }

        public string ClassID { get; set; }

        public string TeacherID { get; set; }

        [Required(ErrorMessage = "Please enter class title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select class date")]
        public string Date { get; set; }

        public string StartTime { get; set; }

        public string ClassEndTime { get; set; }

        [Range(1, 2, ErrorMessage = "Please select class type")]
        [Required(ErrorMessage = "Please select class type")]
        public int SessionType { get; set; }

        public string SessionTypeName { get; set; }

        [MaxLength(1500, ErrorMessage = "Description must be less than 1500 characters")]
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }

        public decimal? Duration { get; set; }

        public string Record { get; set; }

        [Required(ErrorMessage = "Please select subject")]
        public int Subject { get; set; }

        public string SubjectName { get; set; }

        [RequiredIf("SessionType == 1", ErrorMessage = "Please select a time zone")]
        public int? TimeZone { get; set; }

        public int? BrainCertId { get; set; }

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

        public StudentProblemDetailModel ProblemDetail { get; set; }

        public List<Message> Messages { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }
    }
}