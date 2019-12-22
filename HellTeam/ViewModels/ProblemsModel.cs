﻿using System.Web.Mvc;
using ChillLearn.CustomModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Describe your problem")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgProvideDescription")]
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
}