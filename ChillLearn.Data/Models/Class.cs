namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class
    {
        [StringLength(50)]
        public string ClassID { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }

        public int SubjectID { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(10)]
        public string Description { get; set; }

        public int? Type { get; set; }

        public int? ClassDay { get; set; }

        [StringLength(10)]
        public string ClassFrom { get; set; }

        [StringLength(10)]
        public string ClassTo { get; set; }

        public int? Duration { get; set; }

        public int? MaxStudents { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? Status { get; set; }
    }
}
