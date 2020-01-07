
namespace ChillLearn.CustomModels
{
    public class ClassPrice
    {
        public int SubjectID { get; set; }
        public string ClassID { get; set; }
        public decimal? HourlyRate { get; set; }
    }

    public class TeacherLang
    {
        public int ID { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
    }
}
