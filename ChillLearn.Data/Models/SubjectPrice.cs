namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SubjectPrice
    {
        public int ID { get; set; }

        public int? SubjectID { get; set; }

        public decimal? HourlyRate { get; set; }

        public int? StageID { get; set; }

        public decimal? TeacherShare { get; set; }
    }
}
