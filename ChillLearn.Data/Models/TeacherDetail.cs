namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherDetail
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Qualification { get; set; }

        public decimal? YearsExperience { get; set; }

        public int? SubjectID { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }
    }
}
