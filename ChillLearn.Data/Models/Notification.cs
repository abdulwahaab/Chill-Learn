namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Notification
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string FromUser { get; set; }

        [StringLength(50)]
        public string ToUser { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        public int? Type { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ReadingDate { get; set; }
    }
}
