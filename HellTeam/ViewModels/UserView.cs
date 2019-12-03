using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillLearn.ViewModels
{
    public partial class UserView
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MissingEmail")]
        [StringLength(500)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordLength", ErrorMessage = null)]
        [Required(ErrorMessage = null,ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MissingPassword")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "MismatchedPassword", ErrorMessage = null)]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        public string Picture { get; set; }

        [StringLength(50)]
        public string ContactNumber { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        [StringLength(50)]
        public string Class { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Please select a user type.")]
        public int? UserRole { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? Status { get; set; }

        public bool? IsOnline { get; set; }

        public List<SelectListItem> UserRoles { get; set; }
    }

    public class TeacherStageParam
    {
        public string SubjectName { get; set; }
        public int StageId { get; set; }
        //public decimal HourlyRate { get; set; }
    }
    public class QualificationParam
    {
        public string DegreeTitle { get; set; }
        public string InstituteName { get; set; }
        public int YearPassed { get; set; }
    }

    public class TutorRegistration
    {
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please select Country")]
        public string Country { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordLength", ErrorMessage = null)]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MissingPassword")]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "MismatchedPassword", ErrorMessage = null)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please enter contect no")]
        public string ContactNumber { get; set; }
        public string University { get; set; }
        public string HigherQualification { get; set; }
        public string Subject { get; set; }
        public string Experience { get; set; }
        public string SubjectTutored { get; set; }
        public string PreferedTime { get; set; }
        public string Language { get; set; }
        public string LangLevel { get; set; }
        public string AccountNo { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string AccountHolder { get; set; }
        public string Pin { get; set; }

    }
}
