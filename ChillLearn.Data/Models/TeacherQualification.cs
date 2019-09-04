namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherQualification
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }

        [StringLength(150)]
        public string DegreeTitle { get; set; }

        [StringLength(150)]
        public string InstituteName { get; set; }

        public int? YearPassed { get; set; }

        public int? SubjectID { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }
    }
}
