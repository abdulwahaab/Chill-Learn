namespace ChillLearn.ViewModels
{
    using System;

    public partial class StudentCreditLogModel
    {
        public int ID { get; set; }

        public string StudentID { get; set; }

        public string ClassName { get; set; }

        public decimal? CreditsUsed { get; set; }

        public DateTime? CreationDate { get; set; }

        public string LogType { get; set; }
    }

    public partial class TeacherCreditLogModel
    {
        public int ID { get; set; }

        public string TeacherID { get; set; }

        public string ClassName { get; set; }

        public decimal CreditsEarned { get; set; }

        public decimal Funds { get; set; }

        public DateTime CreationDate { get; set; }

        public string LogType { get; set; }
    }
}
