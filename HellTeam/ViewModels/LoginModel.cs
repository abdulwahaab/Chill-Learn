using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChillLearn.ViewModels
{
    public class LoginModel
    {
        //[Required(ErrorMessage = "Please Enter Email")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgEmail")]
        public string UserEmail { get; set; }
        //[Required(ErrorMessage = "Password required")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgPasswordRequried")]
        public string Password { get; set; }

    }

    public class ResetPasswordModel
    {
        //[Required(ErrorMessage = "Password required")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgPasswordRequried")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Confirm password required")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgConfPassRequried")]
        //[Compare("Password", ErrorMessage = "Password doesn't match.")]
        [Compare("Password", ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgPassNotMatch")]
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