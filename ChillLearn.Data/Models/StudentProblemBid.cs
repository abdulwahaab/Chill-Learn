namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentProblemBid
    {
        [Key]
        [StringLength(50)]
        public string BidID { get; set; }

        [StringLength(50)]
        public string ProblemID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? Status { get; set; }
    }
}
