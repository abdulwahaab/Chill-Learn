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

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MissingEmail")]
        [StringLength(500)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MissingPassword")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessageResourceName = "MismatchedPassword")]
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
}
