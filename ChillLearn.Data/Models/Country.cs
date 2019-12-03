namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Country
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Iso { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [StringLength(3)]
        public string Iso3 { get; set; }

        public int? NumCode { get; set; }

        public int PhoneCode { get; set; }
    }
}
