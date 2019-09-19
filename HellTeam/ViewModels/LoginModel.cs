using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChillLearn.ViewModels
{
    public class LoginModel
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }

    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}