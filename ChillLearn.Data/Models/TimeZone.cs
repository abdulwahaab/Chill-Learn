namespace ChillLearn.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class TimeZones
    {
        [Key]
        public int ID { get; set; }

        public int GMT { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
    }
}
