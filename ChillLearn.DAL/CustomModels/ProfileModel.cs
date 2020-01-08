using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.CustomModels
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string ContactNumber { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ProfileImage { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullPhone { get; set; }
    }

    public class TeacherStagesModel
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int? StageId { get; set; }
        public string StageName { get; set; }
        public Decimal? HourlyRate { get; set; }
    }

    public class TeacherSubject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }


    public class TeacherProfileModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ProfileImage { get; set; }
        public string ContactNumber { get; set; }
        public string Title { get; set; }
        public string Qualification { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public TeacherAccountDetail AccountDetail { get; set; }

        //added by Abdul (19-Dec-2019)
        public string MemberSince { get; set; }
        public int ClassesTaught { get; set; }
        public int HoursSpent { get; set; }
        public int QuestionsAnswered { get; set; }

        public string FullPhone { get; set; }
    }
    public class SearchModel
    {
        public string TeacherId { get; set; }
        public int SubjectId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Qualification { get; set; }
        public string Description { get; set; }
    }

    public class TeacherProfileView
    {
        public TeacherProfileModel Profile { get; set; }
        public List<TeacherStagesModel> Subjects { get; set; }
        public List<TeacherQualification> Education { get; set; }
    }

    public class ClassEditModel
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public int BrainCertId { get; set; }
        public int TimeZoneID { get; set; }
        public string TimeZoneName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public string ClassDate { get; set; }
        public string ClassTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal? Duration { get; set; }
        public string Record { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int Status { get; set; }
        public bool? CreatedByStudent { get; set; }
    }

    public class AttendenceReportModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public decimal CreditsUsed { get; set; }
        public string CreditsConsumed { get; set; }
        public string CreditsRefund { get; set; }

        public decimal CreditsConsumedInt { get; set; }
        public decimal CreditsRefundInt { get; set; }
        public int StudentClassId { get; set; }
        public int Status { get; set; }
    }
}
