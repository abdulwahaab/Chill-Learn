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
    }

    public class TeacherStagesModel
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public Decimal? HourlyRate { get; set; }
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
    }

    public class TeacherProfileView
    {
        public TeacherProfileModel Profile { get; set; }
        public List<TeacherStagesModel> Subjects { get; set; }
        public List<TeacherQualification> Education { get; set; }
    }
}
