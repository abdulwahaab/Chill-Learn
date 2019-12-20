using System;
using System.Linq;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;

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
                        join us in context.Users
                        on cls.TeacherID equals us.UserID
                        where (cls.TeacherID == teacherId)
                        select new ClassesModel
                        {
                            Id = cls.Id,
                            ClassId = cls.ClassID,
                            Title = cls.Title,
                            ClassDate = cls.ClassDate,
                            ClassTime = cls.ClassTime,
                            Duration = (int)cls.Duration,
                            SessionType = (int)cls.Type,
                            SubjectName = sb.SubjectName,
                            Status = cls.Status,
                            BrainCertId = cls.BrainCertId,
                            Name = us.FirstName
                        };
            return query.ToList();
        }

        public List<SearchClassModel> SearchClasses(int subjectId, string teacherId, int sessionType, string studentId,int canceled)
        {
            string sqlQuery = "select c.ClassID,u.UserID,Title,ClassDate as ClassDate,ClassTime as ClassTime,c.BrainCertId,Duration,SubjectName,Type as SessionType,sc.Status as StatusJoin from Classes c"
                                + " inner join Subjects sb on sb.SubjectID = c.SubjectID"
                                + " left join StudentClasses sc on sc.ClassID = c.ClassID"
                                + " and (sc.StudentID = '" + studentId + "'"
                                + "or sc.StudentID IS NULL) left join Users u on u.UserID = sc.StudentID where c.Status != "+ canceled + " and c.ClassDate > '"+DateTime.Now+"'";
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
                            //StageId = st.StageID,
                            StageName = st.StageName
                            //HourlyRate = ts.HourlyRate
                        };
            return query.ToList();
        }

        public TeacherProfileModel GetTeacherProfile(string teacherId)
        {
            try
            {
                var query = from user in context.Users
                            join td in context.TeacherDetails
                           on user.UserID equals td.TeacherID into gj
                            from x in gj.DefaultIfEmpty()
                            where (user.UserID == teacherId)
                            select new TeacherProfileModel
                            {
                                UserId = user.UserID,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                Picture = user.Picture,
                                Address = user.Address,
                                Country = user.Country,
                                City = user.City,
                                ProfileImage = user.Picture,
                                ContactNumber = user.ContactNumber,
                                Title = x.University,
                                Qualification = x.Qualification,
                                Description = x.Description,
                                Experience = x.YearsExperience

                            };
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<ClassEditModel> GetClassData(string classId)
        {
            var query = from cls in context.Classes
                        join sub in context.Subjects
                       on cls.SubjectID equals sub.SubjectID
                        where (cls.ClassID == classId)
                        select new ClassEditModel
                        {
                            Id = cls.Id,
                            ClassId = cls.ClassID,
                            BrainCertId = cls.BrainCertId,
                            ClassDate = cls.ClassDate,
                            ClassTime = cls.ClassTime,
                            Description = cls.Description,
                            Duration = cls.Duration,
                            Record = cls.Record,
                            SubjectName = sub.SubjectName,
                            Title = cls.Title,
                            Type = cls.Type
                        };
            return query.ToList();
        }
    }
}