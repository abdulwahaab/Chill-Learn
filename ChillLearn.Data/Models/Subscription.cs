namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Subscription
    {
        [StringLength(50)]
        public string SubscriptionID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        public decimal? Hours { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }
    }
}
