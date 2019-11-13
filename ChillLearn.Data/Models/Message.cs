namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {
        public int MessageID { get; set; }

        [StringLength(50)]
        public string FromUser { get; set; }

        [StringLength(50)]
        public string ToUser { get; set; }

        [StringLength(50)]
        public string BidID { get; set; }

        [Column("Message")]
        public string Message1 { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ViewedDate { get; set; }

        public int? Status { get; set; }
    }
}
