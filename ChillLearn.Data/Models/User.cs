namespace ChillLearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public int AutoID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(500)]
        public string NationalID { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Picture { get; set; }

        [StringLength(50)]
        public string ContactNumber { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        [StringLength(50)]
        public string Class { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        public int? UserRole { get; set; }

        public int? Source { get; set; }

        public string ValidationToken { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? Status { get; set; }

        public bool? IsOnline { get; set; }

        [StringLength(100)]
        public string ConnectionId { get; set; }
    }
}
