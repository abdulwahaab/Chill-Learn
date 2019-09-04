namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public decimal? UnitPrice { get; set; }

        [StringLength(50)]
        public string FBURL { get; set; }

        [StringLength(50)]
        public string TwitterURL { get; set; }

        public decimal? FeaturedClassPrice { get; set; }

        public decimal? FeaturedTeacherPrice { get; set; }
    }
}
