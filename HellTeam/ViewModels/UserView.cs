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

        [Required(ErrorMessage = "Please provide email address.")]
        [StringLength(500)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide password.")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "Confirm password doesn't match, Type again !")]
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
        public decimal HourlyRate { get; set; }
    }
}
