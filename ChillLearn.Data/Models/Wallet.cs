namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wallet")]
    public partial class Wallet
    {
        public int ID { get; set; }

        public int? PaymentID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public decimal? Hours { get; set; }

        public decimal? Funds { get; set; }

        [StringLength(10)]
        public string TransactionType { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }
    }
}
