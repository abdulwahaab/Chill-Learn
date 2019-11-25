namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherCertification
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TeacherId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Institute { get; set; }

        public int? Year { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
