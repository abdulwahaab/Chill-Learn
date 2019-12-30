using System.Linq;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;

namespace ChillLearn.DAL
{
    public class StudentRepository
    {
        internal ChillLearnContext context;

        public StudentRepository(ChillLearnContext context)
        {
            this.context = context;
        }

        //public List<StudentClasses> GetClasses(string studentId)
        //{
        //    var query = from sc in context.StudentClasses
        //                join c in context.Classes on sc.ClassID equals c.ClassID
        //                join sb in context.Subjects on c.SubjectID equals sb.SubjectID
        //                join us in context.Users on sc.StudentID equals us.UserID
        //                where (sc.StudentID == studentId)
        //                select new StudentClasses
        //                {
        //                    Id = sc.ID,
        //                    ClassId = c.ClassID,
        //                    Title = c.Title,
        //                    ClassDate = c.ClassDate,
        //                    ClassTime = c.ClassTime,
        //                    SessionType = (int)c.Type,
        //                    SubjectName = sb.SubjectName,
        //                    ClassStatus = c.Status,
        //                    RequestStatus = sc.Status,
        //                    Record = c.Record,
        //                    TeacherId = c.TeacherID,
        //                    BrainCertId = c.BrainCertId,
        //                    Name = us.FirstName
        //                    //CombDT =  new DateTime(c.ClassDate.Year, c.ClassDate.Month, c.ClassDate.Day, c.ClassTime.Hours, c.ClassTime.Minutes,c.ClassTime.Seconds)
        //                };
        //    return query.ToList();
        //}

        public List<StudentClasses> GetClasses(string studentId)
        {
            //string query = @"select u.AutoID as Id, class.ClassID, class.Title, class.ClassDate, class.StartTime as ClassTime, class.Type as SessionType, 
            //sub.SubjectName, class.Status as ClassStatus, sc.Status as RequestStatus, class.Record, class.TeacherID, 
            //class.BrainCertId, u.FirstName as [Name]
            //from Classes as class
            //left join StudentClasses as sc on class.ClassID = sc.ClassID
            //inner join Subjects as sub on class.SubjectID = sub.SubjectID
            //left join Users as u on(sc.StudentID = u.UserID or class.CreatedBy = u.UserID)
            //where sc.StudentID='" + studentId + "' or class.CreatedBy = '" + studentId + "'";

            string query = @"select u.AutoID as UserID, class.ClassID, class.Title, class.ClassDate, class.StartTime as ClassTime, class.Type as SessionType, 
            sub.SubjectName, class.Status as ClassStatus, sc.Status as RequestStatus, class.Record, class.TeacherID, 
            class.BrainCertId, u.FirstName as [Name]
            from Classes as class
            left join StudentClasses as sc on class.ClassID = sc.ClassID
            inner join Subjects as sub on class.SubjectID = sub.SubjectID
            left join Users as u on(sc.StudentID = u.UserID)
            where sc.StudentID='" + studentId + "' or class.CreatedBy = '" + studentId + "'";

            return context.Database.SqlQuery<StudentClasses>(query)
                .ToList();
        }

        public ClassDetail GetClassDetail(int brainCertClassId)
        {
            var query = from c in context.Classes
                        join sub in context.Subjects on c.SubjectID equals sub.SubjectID
                        where c.BrainCertId == brainCertClassId
                        select new ClassDetail
                        {
                            BrainCertClassID = (int)c.BrainCertId,
                            ClassID = c.ClassID,
                            SubjectName = sub.SubjectName,
                            Title = c.Title
                        };
            return query.ToList().FirstOrDefault();
        }
    }
}
