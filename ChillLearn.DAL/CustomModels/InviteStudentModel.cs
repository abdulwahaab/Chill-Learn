using System;
using ChillLearn.Data.Models;
using System.Collections.Generic;

namespace ChillLearn.CustomModels
{
    public class InviteStudentModel
    {
        public string ClassTitle { get; set; }
        public string ClassID { get; set; }
        public string SearchKeyword { get; set; }
        public string StudentID { get; set; }
        public string TeacherID { get; set; }
        public List<User> Students { get; set; }
        public DateTime InviteDate { get; set; }
    }
}
