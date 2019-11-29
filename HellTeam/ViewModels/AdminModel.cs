using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChillLearn.ViewModels
{
    public class AdminModel
    {
    }
    public class PlanParam
    {
        [Required(ErrorMessage = "Please provide price")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Please provide plan name")]
        public string PlanName { get; set; }
    }
}