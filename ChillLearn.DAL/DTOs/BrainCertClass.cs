
namespace ChillLearn.DAL
{
    public class BrainCertClass
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string IsRecord { get; set; }
    }

    public class BrainCertClassSuccess
    {
        public string Status { get; set; }
        public string method { get; set; }
        public string class_id { get; set; }
        public string title { get; set; }
    }

    public class BrainCertClassError
    {
        public string status { get; set; }
        public string error { get; set; }
    }
}