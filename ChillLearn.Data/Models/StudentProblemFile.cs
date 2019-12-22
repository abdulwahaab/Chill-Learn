namespace ChillLearn.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class StudentProblemFile
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string ProblemID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public string FileName { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
