namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Claim
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClaimID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
