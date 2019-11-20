namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClassFile
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ClassId { get; set; }

        [StringLength(100)]
        public string Image { get; set; }
    }
}
