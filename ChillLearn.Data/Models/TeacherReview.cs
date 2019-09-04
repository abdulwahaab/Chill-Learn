namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherReview
    {
        [Key]
        [StringLength(50)]
        public string ReviewID { get; set; }

        [StringLength(50)]
        public string TutorID { get; set; }

        [StringLength(50)]
        public string StudentID { get; set; }

        [Required]
        [StringLength(50)]
        public string ProblemID { get; set; }

        public decimal? Hours { get; set; }

        public int? Rating { get; set; }

        [StringLength(500)]
        public string Feedback { get; set; }

        public bool? IsRecommended { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? Status { get; set; }
    }
}
