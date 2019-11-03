using System;
using System.Collections.Generic;

namespace ChillLearn.Data
{
    public partial class Classes
    {
        public string ClassId { get; set; }
        public string CreatedBy { get; set; }
        public string TeacherId { get; set; }
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public int? ClassDay { get; set; }
        public DateTime? ClassFrom { get; set; }
        public string ClassTo { get; set; }
        public int? Duration { get; set; }
        public bool? Record { get; set; }
        public int? MaxStudents { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}