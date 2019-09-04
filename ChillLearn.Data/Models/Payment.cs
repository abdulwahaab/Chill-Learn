namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payment
    {
        public int PaymentID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string SubscriptionID { get; set; }

        public int? Type { get; set; }

        public decimal? Amount { get; set; }

        public int? Source { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }
    }
}
