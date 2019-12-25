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
                        join sb in context.Subjects on cls.SubjectID equals sb.SubjectID
                        join us in context.Users on cls.TeacherID equals us.UserID
                        join creator in context.Users on cls.CreatedBy equals creator.UserID
                        where cls.TeacherID == teacherId &&
                        (cls.Status != (int)ClassStatus.BidAccepted && cls.Status != (int)ClassStatus.OfferDeclined)
                        select new ClassesModel
                        {
                            Id = cls.Id,
                            ClassId = cls.ClassID,
                            Title = cls.Title,
                            ClassDate = cls.ClassDate,
                            ClassTime = cls.StartTime,
                            Duration = cls.Duration,
                            SessionType = (int)cls.Type,
                            SubjectName = sb.SubjectName,
                            Status = cls.Status,
                            BrainCertId = cls.BrainCertId,
                            TeacherName = us.FirstName + " " + us.LastName,
                            CreatorName = creator.FirstName + " " + creator.LastName
                        };
            return query.ToList();
        }

        public List<SearchClassModel> SearchClasses(int subjectId, string teacherId, int sessionType, string studentId, int canceled, string dateNow, string keyword)
        {
            string sqlQuery = @"select c.ClassID, u.UserID, tu.Picture as TeacherPicture, Title, ClassDate as ClassDate, StartTime as ClassTime, c.BrainCertId, Duration,
            SubjectName,Type as SessionType,sc.Status as StatusJoin from Classes c 
            inner join Subjects sb on sb.SubjectID = c.SubjectID 
            left join StudentClasses sc on sc.ClassID = c.ClassID and (sc.StudentID = '" + studentId + "' or sc.StudentID IS NULL) " +
            "left join Users u on u.UserID = sc.StudentID " +
            "left join Users tu on tu.UserID = c.TeacherID " +
            "where c.Type != " + (int)SessionType.Written + " and c.Status != " + canceled + " and c.ClassDate > '" + dateNow + "' and c.CreatedByStudent=0 and " +
            "(sb.SubjectName like '%" + keyword + "%' or c.Title like '%" + keyword + "%' or c.Description like '%" + keyword + "%') ";
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

        public ClassEditModel GetClassData(string classId)
        {
            var query = from cls in context.Classes
                        join sub in context.Subjects
                       on cls.SubjectID equals sub.SubjectID
                        where (cls.ClassID == classId)
                        select new ClassEditModel
                        {
                            Id = cls.Id,
                            ClassId = cls.ClassID,
                            BrainCertId = (int)cls.BrainCertId,
                            ClassDate = cls.ClassDate.ToString(),
                            ClassTime = cls.StartTime,
                            Description = cls.Description,
                            Duration = cls.Duration,
                            Record = cls.Record.ToString(),
                            SubjectName = sub.SubjectName,
                            Title = cls.Title,
                            Type = cls.Type,
                            SubjectId = sub.SubjectID,
                            CreatedByStudent = cls.CreatedByStudent,
                            Status = (int)cls.Status
                        };
            return query.FirstOrDefault();
        }

        //public List<SelectDate> GetUserInfo(List<int> userIds)
        //{
        //    string sqlQuery = "select * from StudentClasses c "
        //                      + " inner join Users u on u.UserID = c.StudentID"
        //                      + " inner join StudentCreditLog cl on cl.ClassID = c.ClassID and cl.StudentID = u.UserID"
        //                       + " where c.ID IN("+ string.Join(",", userIds) + ")";
        //    var results = context.Database.SqlQuery<SelectDate>(sqlQuery).ToList();
        //    return results;
        //}

        public AttendenceReportModel GetUserInfo(int userId, string classId)
        {
            string sqlQuery = @"select * from StudentCreditLog as cl
                                left join Users as u on cl.UserID = u.UserID 
                                left join Classes as c on cl.ClassID = c.ClassID
                                left join StudentClasses sc on cl.ClassID = sc.ClassID and cl.UserID = u.UserID 
                                where u.AutoID = " + userId + " and c.ClassID='" + classId + "'";
            var results = context.Database.SqlQuery<AttendenceReportModel>(sqlQuery).FirstOrDefault();
            return results;
        }

        public ClassPrice GetClassSubjectRate(string classId)
        {
            var query = from sub in context.Subjects
                        join cls in context.Classes on sub.SubjectID equals cls.SubjectID
                        where cls.ClassID == classId
                        select new ClassPrice
                        {
                            ClassID = cls.ClassID,
                            SubjectID = sub.SubjectID,
                            HourlyRate = sub.HourlyRate
                        };
            return query.FirstOrDefault();
        }

        //public AttendenceReportModel GetUserInfo(int userId, int approved)
        //{
        //    string sqlQuery = "select * from StudentClasses c "
        //                      + " inner join Users u on u.UserID = c.StudentID"
        //                      + " inner join StudentCreditLog cl on cl.ClassID = c.ClassID and cl.StudentID = u.UserID"
        //                      + " where c.ID  = " + userId + " and c.Status = " + approved + "";
        //    var results = context.Database.SqlQuery<AttendenceReportModel>(sqlQuery).FirstOrDefault();
        //    return results;
        //}
    }
}