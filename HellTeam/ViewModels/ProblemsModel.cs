using System.Web.Mvc;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChillLearn.Data.Models;
using System.Web;
using System;

namespace ChillLearn.ViewModels
{
    public partial class ProblemsModel
    {
        [Display(Name = "Select Type")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgSelectType")]
        public int Type { get; set; }

        [Display(Name = "Select Subject")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgSelectSubject")]
        public int Subject { get; set; }

        [Display(Name = "Hours Needed")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgHoursNeeded")]
        public decimal HoursNeeded { get; set; }

        [Display(Name = "Set Deadline")]
        public string DeadLine { get; set; }

        //[Display(Name = "Describe your problem")]
        //[Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgProvideDescription")]
        public string TeacherID { get; set; }

        public string ProblemDescription { get; set; }

        public List<SelectListItem> SessionTypes { get; set; }

        public SelectList Subjects { get; set; }

        public List<StudentProblemsModel> Problems { get; set; }
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

        //[Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Please Select Date")]
        public string Date { get; set; }

        //[Required(ErrorMessage = "Please Select Time")]
        public string StartTime { get; set; }

        public string ClassEndTime { get; set; }

        //[Required(ErrorMessage = "Please Select Session")]
        public int SessionType { get; set; }

        //[Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Please Enter Duration")]
        [Range(0.5, Double.MaxValue)]
        public decimal Duration { get; set; }

        public string Record { get; set; }

        //[Required(ErrorMessage = "Please Select Subject")]
        public int Subject { get; set; }

        public int? BrainCertId { get; set; }

        public List<SelectListItem> SessionTypes { get; set; }

        public SelectList Subjects { get; set; }

        public StudentProblemDetailModel ProblemDetail { get; set; }

        public List<Message> Messages { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }
    }
}