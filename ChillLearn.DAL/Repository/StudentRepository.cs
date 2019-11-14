using ChillLearn.CustomModels;
using ChillLearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.DAL
{
    public class StudentRepository
    {
        internal ChillLearnContext context;

        public StudentRepository(ChillLearnContext context)
        {
            this.context = context;
        }
        public List<StudentClasses> GetClasses(string studentId,int status)
        {
            var query = from sc in context.StudentClasses
                        join c in context.Classes
                      on sc.ClassID equals c.ClassID
                        join sb in context.Subjects
                       on c.SubjectID equals sb.SubjectID
                        where (sc.StudentID == studentId && sc.Status == status)
                        select new StudentClasses
                        {
                            ClassId = c.ClassID,
                            Title = c.Title,
                            ClassDate = c.ClassFrom,
                            ClassTime = c.ClassTo,
                            SessionType = (int)c.Type,
                            SubjectName = sb.SubjectName,
                            ClassStatus = c.Status,
                            Record = c.Record,
                            TeacherId = c.TeacherID
                        };
            return query.ToList();
        }
    }
}
