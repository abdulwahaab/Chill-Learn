using ChillLearn.CustomModels;
using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.ViewModels
{
    public partial class ProblemsModel
    {

        [Display(Name = "Select Type")]
        [Required(ErrorMessage = "Please select type.")]
        public int Type { get; set; }
        [Display(Name = "Select Subject")]
        [Required(ErrorMessage = "Please select subject.")]
        public int Subject { get; set; }
        [Display(Name = "Hours Needed")]
        [Required(ErrorMessage = "Please provide Hours Needed.")]
        public decimal HoursNeeded { get; set; }
        [Display(Name = "Set Deadline")]
        public string DeadLine { get; set; }
        [Display(Name = "Describe your problem")]
        [Required(ErrorMessage = "Please provide some problem description.")]
        public string ProblemDescription { get; set; }
        public List<SelectListItem> SessionTypes { get; set; }
        public SelectList Subjects { get; set; }
        public List<StudentProblemsModel> Problems { get; set; }

    }

    public partial class ProblemDetailModel
    {

        [Display(Name = "Response")]
        [Required(ErrorMessage = "Please provide some response description.")]
        public string Response { get; set; }
        [Required]
        public string ProblemId { get; set; }
        public QuestionModel ProblemDetails { get; set; }

    }

    public partial class QuestionDetailModel
    {

        [Display(Name = "Write Proposal")]
        [Required(ErrorMessage = "Please provide some proposal description.")]
        public string Response { get; set; }
        [Required]
        public string ProblemId { get; set; }
        public QuestionModel QuestionDetail { get; set; }

    }
}