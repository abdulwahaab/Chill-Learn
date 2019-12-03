namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeacherAccountDetail")]
    public partial class TeacherAccountDetail
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TeacherId { get; set; }

        [StringLength(50)]
        public string AccountNo { get; set; }

        [StringLength(50)]
        public string BranchName { get; set; }

        [StringLength(50)]
        public string BranchCode { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string PinNo { get; set; }

        public int? Status { get; set; }
    }
}
