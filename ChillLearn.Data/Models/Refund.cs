namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Refund
    {
        public int RefundID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string SubscriptionID { get; set; }

        public decimal? Amount { get; set; }

        public string Reason { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ApproveDate { get; set; }

        public int? Status { get; set; }
    }
}
