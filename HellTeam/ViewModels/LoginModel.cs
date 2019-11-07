﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChillLearn.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password required")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmail { get; set; }

    }
}