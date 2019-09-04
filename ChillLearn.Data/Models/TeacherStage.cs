namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherStage
    {
        public int ID { get; set; }

        public int SubjectID { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }

        public int? StageID { get; set; }

        public decimal? HourlyRate { get; set; }
    }
}
