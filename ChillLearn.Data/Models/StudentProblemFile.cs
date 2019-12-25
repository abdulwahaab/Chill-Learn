namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentProblemFile
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ProblemID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
