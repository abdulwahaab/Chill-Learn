
using ChillLearn.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChillLearn.ViewModels
{
    public class ClassModel
    {
    }
    public class ClassViewModel
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int SessionType { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public bool Record { get; set; }
        public int Subject { get; set; }
        public List<SelectListItem> SessionTypes { get; set; }
        public SelectList Subjects { get; set; }
    }

    public class ClassFindParam
    {
        public SelectList Teachers { get; set; }
        public SelectList Subjects { get; set; }
        public List<SelectListItem> SessionTypes { get; set; }
        public SearchParam Search{ get; set; }
        public List<SearchClassModel> Classes { get; set; }

    }
    public class SearchParam
    {
        public string TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int SessionType { get; set; }
    }

    public class JoinParam
    {
        public string ClassId { get; set; }
    }
}