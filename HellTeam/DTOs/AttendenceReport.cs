
namespace ChillLearn.Controllers
{
    public class AttendenceReport
    {
        public string classId { get; set; }
        public int userId { get; set; }
        public string duration { get; set; }
        public string percentage { get; set; }
        public string attendance { get; set; }
        public int isTeacher { get; set; }
    }
}