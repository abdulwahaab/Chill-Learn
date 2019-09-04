namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentClass
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string StudentID { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassID { get; set; }

        public DateTime? JoiningDate { get; set; }

        public int? Status { get; set; }
    }
}
