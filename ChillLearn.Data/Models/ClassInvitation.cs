namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClassInvitation
    {
        [Key]
        public int InvitationID { get; set; }

        [StringLength(50)]
        public string ClassID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }
    }
}
