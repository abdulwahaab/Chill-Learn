namespace ChillLearn.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Language
    {
        public int ID { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
