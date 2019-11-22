using ChillLearn.CustomModels;
using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.DAL
{
   
    public class TeacherRepository
    {
        internal ChillLearnContext context;
        public TeacherRepository(ChillLearnContext context)
        {
            this.context = context;
        }
        public List<ClassesModel> GetClasses(string teacherId)
        {
            var query = from cls in context.Classes
                        join sb in context.Subjects
                       on cls.SubjectID equals sb.SubjectID
                        where (cls.TeacherID == teacherId)
                        select new ClassesModel
                        {
                            ClassId = cls.ClassID,
                            Title = cls.Title,
                            ClassDate = cls.ClassFrom,
                            ClassTime = cls.ClassTo,
                            Duration = (int)cls.Duration,
                            SessionType = (int)cls.Type,
                            SubjectName = sb.SubjectName,
                            Status = cls.Status};
            return query.ToList();
        }
        public List<SearchClassModel> SearchClasses(int subjectId, string teacherId, int sessionType,string studentId)
        {
            string sqlQuery = "select c.ClassID,u.UserID,Title,ClassFrom as ClassDate,ClassTo as ClassTime,Duration,SubjectName,Type as SessionType,sc.Status as StatusJoin from Classes c"
                                + " inner join Subjects sb on sb.SubjectID = c.SubjectID"
                                + " left join StudentClasses sc on sc.ClassID = c.ClassID"
								+" and (sc.StudentID = '"+studentId +"'"
                                + "or sc.StudentID IS NULL) left join Users u on u.UserID = sc.StudentID where c.Status != 3";
            //bool checkDone = false;
            if (subjectId != 0)
            {
                //checkDone = true;
                sqlQuery += "and c.SubjectId = " + subjectId + " ";
            }
            if (!string.IsNullOrEmpty(teacherId))
            {
                //checkDone = true;
                sqlQuery += "and c.TeacherID = '" + teacherId + "' ";
            }
            if (sessionType != 0)
            {
                //checkDone = true;
                sqlQuery += "and c.Type = " + sessionType + " ";
            }
            string queryStr = sqlQuery;
            //if (checkDone)
            //{
            //    queryStr = queryStr.Remove(queryStr.Length - 4);
            //}
            var results = context.Database.SqlQuery<SearchClassModel>(queryStr)
                .ToList();
            return results;
        }
        public List<RequestsModel> GetClassRequests(string classId)
        {
            var query = from cls in context.StudentClasses
                        join u in context.Users
                       on cls.StudentID equals u.UserID
                        join cl in context.Classes
                     on cls.ClassID equals cl.ClassID
                        where (cls.ClassID == classId)
                        select new RequestsModel
                        {
                            Id = cls.ID,
                            ClassId = cls.ClassID,
                            RequestStatus = cls.Status,
                            StudentId = cls.StudentID,
                            StudentName = u.FirstName,
                            RequestDate = cls.JoiningDate,
                            ClassTitle = cl.Title,
                            ProfilePicture = u.Picture
                        };
            return query.ToList();
        }

        public List<TeacherStagesModel> GetTeacherStages(string teacherId)
        {
            var query = from ts in context.TeacherStages
                        join sub in context.Subjects
                       on ts.SubjectID equals sub.SubjectID
                        join st in context.Stages
                     on ts.StageID equals st.StageID
                        where (ts.TeacherID == teacherId)
                        select new TeacherStagesModel
                        {
                            Id = ts.ID,
                            SubjectId = sub.SubjectID,
                            SubjectName = sub.SubjectName,
                            StageId = st.StageID,
                            StageName = st.StageName,
                            HourlyRate = ts.HourlyRate
                        };
            return query.ToList();
        }


    }
}
