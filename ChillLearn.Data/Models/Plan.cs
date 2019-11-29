namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Plan
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(150)]
        public string PlanName { get; set; }

        public decimal Price { get; set; }

        public int? Status { get; set; }

        public int? Credits { get; set; }
    }
}
