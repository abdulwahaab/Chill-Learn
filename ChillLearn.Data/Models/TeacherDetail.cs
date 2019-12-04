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
        public string University { get; set; }

        [StringLength(250)]
        public string Qualification { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string YearsExperience { get; set; }

        [StringLength(50)]
        public string SubjectExperties { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string PreferedTime { get; set; }
    }
}
