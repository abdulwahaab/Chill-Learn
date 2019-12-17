namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentCredit
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string StudentID { get; set; }

        public decimal? TotalCredits { get; set; }

        public decimal? UsedCredits { get; set; }

        public DateTime? LastUpdates { get; set; }
    }
}
