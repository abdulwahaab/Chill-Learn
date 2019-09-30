namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentProblem
    {
        [Key]
        [StringLength(50)]
        public string ProblemID { get; set; }

        [StringLength(50)]
        public string StudentID { get; set; }

        public int? Type { get; set; }

        public int? SubjectID { get; set; }

        public string FileName { get; set; }

        public decimal? HoursNeeded { get; set; }

        public decimal? HoursSpent { get; set; }

        public string Description { get; set; }

        public DateTime? ExpireDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? Status { get; set; }
    }
}
