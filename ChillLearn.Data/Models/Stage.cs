namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stage
    {
        public int StageID { get; set; }

        [StringLength(50)]
        public string StageName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? Status { get; set; }
    }
}
