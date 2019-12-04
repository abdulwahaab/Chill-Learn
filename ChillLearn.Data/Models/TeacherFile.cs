namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeacherFile
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string TeacherID { get; set; }
        public string FileName { get; set; }

        public byte? Type { get; set; }
    }
}
