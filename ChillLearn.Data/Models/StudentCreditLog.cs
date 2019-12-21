namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentCreditLog")]
    public partial class StudentCreditLog
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string StudentID { get; set; }

        public string ClassID { get; set; }

        public decimal? CreditsUsed { get; set; }

        public DateTime? CreationDate { get; set; }

        public string LogType { get; set; }
    }
}
