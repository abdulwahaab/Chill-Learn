namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherLanguage
    {
        public int ID { get; set; }

        public int? Language { get; set; }

        public int? LangLevel { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }
    }
}
