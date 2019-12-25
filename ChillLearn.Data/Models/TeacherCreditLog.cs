namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeacherCreditLog")]
    public partial class TeacherCreditLog
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }

        [StringLength(50)]
        public string ClassID { get; set; }

        public decimal CreditsEarned { get; set; }

        public decimal Funds { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(50)]
        public string LogType { get; set; }
    }
}
