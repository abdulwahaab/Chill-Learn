using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.CustomModels
{
    public class StudentProblemsModel
    {
        public string ProblemID { get; set; }
        public string SubjectName { get; set; }
        public string HoursNeeded { get; set; }
        public string ProblemDescription { get; set; }
        public int? Type { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? CreationDate { get; set; }
    }
    public class StudentProblemDetailModel
    {
        public string ProblemID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ProblemDescription { get; set; }
        public string TeacherResponse { get; set; }
        public int? Type { get; set; }
        public DateTime? ProblemDate { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
    public class QuestionModel
    {
        public string ProblemID { get; set; }
        public string UserID { get; set; }
        public decimal? HoursNeeded { get; set; }
        public int? Type { get; set; }
        public string SubjectName { get; set; }
        public string UserName { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? CreationDate { get; set; }
    }
    public class BidsModel
    {
        public string BidId { get; set; }
        public string ProblemId { get; set; }
        public string UserId { get; set; }
        public string ProposalDescription { get; set; }
        public string UserName { get; set; }
        public string UserProfile { get; set; } //chnage on 19-11-2019 byte[] to string
    }
    public class BidDetailModel
    {
        [Display(Name = "Response")]
        [Required(ErrorMessage = "Please provide some response description.")]
        public string Response { get; set; }
        [Required]
        public string BidId { get; set; }
        public string ToUser { get; set; }
        public StudentProblemDetailModel ProblemDetail { get; set; }
        public List<Message> Messages { get; set; }

    }
    public class ClassesModel
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "date")]
        public DateTime ClassDate { get; set; }
        public string ClassTime { get; set; }
        public int Duration { get; set; }
        public string SubjectName { get; set; }
        public int SessionType { get; set; }
        public int? Status { get; set; }
        public int BrainCertId { get; set; }
        public string Name { get; set; }
    }
    public class SearchClassModel
    {
        public string ClassId { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public DateTime ClassDate { get; set; }
        public string ClassTime { get; set; }
        public decimal Duration { get; set; }
        public string SubjectName { get; set; }
        public int SessionType { get; set; }
        public int? StatusJoin { get; set; }
        public int BrainCertId { get; set; }
    }
    public class UserIdName
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
    public class StudentClasses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassId { get; set; }
        public string TeacherId { get; set; }
        public string Title { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ClassDate { get; set; }
        public string ClassTime { get; set; }
        public string SubjectName { get; set; }
        public int SessionType { get; set; }
        public int? ClassStatus { get; set; }
        public bool? Record { get; set; }
        public int BrainCertId { get; set; }
        public DateTime CombDT { get; set; }
    }

}
