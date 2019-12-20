using System;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using ChillLearn.Data.Models;
using ChillLearn.CustomModels;
using System.Collections.Generic;

namespace ChillLearn.DAL
{
    public class UserRepository
    {
        internal ChillLearnContext context;

        public UserRepository(ChillLearnContext context)
        {
            this.context = context;
        }

        public User GetUserLogin(string email, string password, int source)
        {
            return context.Users.Where(u => u.Email == email && u.Password == password && u.Source == source).FirstOrDefault();
        }

        public User GetUserFacebookLogin(string email, int source, int status)
        {
            return context.Users.Where(u => u.Email == email && u.Source == source && u.Status == status).FirstOrDefault();
        }

        public void SendEmail(string email, string subject, string emailHtml)
        {
            try
            {
                string emailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                string smtpClient = ConfigurationManager.AppSettings["SmtpClient"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtpClient);

                mail.From = new MailAddress(emailAddress);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = emailHtml;
                mail.IsBodyHtml = true;

                SmtpServer.Port = port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(emailAddress, password);
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }

        public bool UpdateUserStatus(string validationToken, int status)
        {
            try
            {
                var result = context.Users.SingleOrDefault(b => b.ValidationToken == validationToken);
                if (result != null)
                {
                    result.Status = status;
                    result.ValidationToken = "";
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ForgotPassword(string token, string email)
        {
            try
            {
                var result = context.Users.SingleOrDefault(b => b.Email == email);
                if (result != null)
                {
                    result.ValidationToken = token;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public User GetUserByToken(string validationToken)
        {
            try
            {
                var result = context.Users.SingleOrDefault(b => b.ValidationToken == validationToken);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool UpdadeUserPassword(string password, string token)
        {
            try
            {
                User result = context.Users.SingleOrDefault(b => b.ValidationToken == token);
                if (result != null)
                {
                    result.ValidationToken = "";
                    result.Password = password;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User GetTeacherProfile(string userId)
        {
            var query = from user in context.Users
                        join teacher in context.TeacherDetails
                        on user.UserID equals teacher.TeacherID
                        where (user.UserID == userId)
                        select new
                        {
                            aaa = user.UserID
                        };

            return null;
        }

        public List<StudentProblemsModel> GetProblemsByStudentId(string Id)
        {
            var query = from sp in context.StudentProblems
                        join sub in context.Subjects
                        on sp.SubjectID equals sub.SubjectID
                        where (sp.StudentID == Id)
                        orderby sp.CreationDate descending
                        select new StudentProblemsModel
                        {
                            ProblemID = sp.ProblemID,
                            CreationDate = sp.CreationDate,
                            ExpireDate = sp.ExpireDate,
                            HoursNeeded = sp.HoursNeeded.ToString(),
                            ProblemDescription = sp.Description,
                            SubjectName = sub.SubjectName,
                            Type = sp.Type
                        };
            return query.ToList();
        }

        public List<StudentProblemsModel> GetProblems(string userId)
        {
            List<TeacherStage> teacherSubjects = context.TeacherStages.Where(x => x.TeacherID == userId).ToList();
            List<StudentProblem> filteredProblems = new List<StudentProblem>();
            foreach (var subject in teacherSubjects)
            {
                var subjectProblems = context.StudentProblems.Where(p => p.SubjectID == subject.SubjectID &&
                string.IsNullOrEmpty(p.TeacherID) &&
                !context.StudentProblemBids.Where(x => x.UserID == userId).Any(x => x.ProblemID == p.ProblemID)).ToList();
                filteredProblems.AddRange(subjectProblems);
            }
            var query = from sp in filteredProblems
                        join sub in context.Subjects
                        on sp.SubjectID equals sub.SubjectID
                        orderby sp.CreationDate descending
                        select new StudentProblemsModel
                        {
                            ProblemID = sp.ProblemID,
                            CreationDate = sp.CreationDate,
                            ExpireDate = sp.ExpireDate,
                            HoursNeeded = sp.HoursNeeded.ToString(),
                            ProblemDescription = sp.Description,
                            SubjectName = sub.SubjectName,
                            Type = sp.Type
                        };
            return query.ToList();
        }

        public List<StudentProblemsModel> GetQuestionRequests(string userId)
        {
            List<TeacherStage> teacherSubjects = context.TeacherStages.Where(x => x.TeacherID == userId).ToList();
            List<StudentProblem> filteredProblems = new List<StudentProblem>();
            foreach (var subject in teacherSubjects)
            {
                var subjectProblems = context.StudentProblems.Where(p => p.SubjectID == subject.SubjectID &&
                p.TeacherID == userId &&
                !context.StudentProblemBids.Where(x => x.UserID == userId).Any(x => x.ProblemID == p.ProblemID)).ToList();
                filteredProblems.AddRange(subjectProblems);
            }
            var query = from sp in filteredProblems
                        join sub in context.Subjects
                        on sp.SubjectID equals sub.SubjectID
                        orderby sp.CreationDate descending
                        select new StudentProblemsModel
                        {
                            ProblemID = sp.ProblemID,
                            CreationDate = sp.CreationDate,
                            ExpireDate = sp.ExpireDate,
                            HoursNeeded = sp.HoursNeeded.ToString(),
                            ProblemDescription = sp.Description,
                            SubjectName = sub.SubjectName,
                            Type = sp.Type
                        };
            return query.ToList();
        }

        public StudentProblemDetailModel GetProblemDetailByBidId(string bidId, string userId)
        {
            var query = from sp in context.StudentProblems
                        join spb in context.StudentProblemBids on sp.ProblemID equals spb.ProblemID /*into sasa*/
                        //from spbd in sasa.DefaultIfEmpty()
                        join pp in context.Users on spb.UserID equals pp.UserID

                        where (spb.BidID == bidId /*&& spb.UserID == userId*/)
                        orderby spb.CreationDate ascending
                        select new StudentProblemDetailModel
                        {
                            ProblemID = sp.ProblemID,
                            ProblemDate = sp.CreationDate,
                            ResponseDate = spb.CreationDate,
                            TeacherResponse = spb.Description,
                            ProblemDescription = sp.Description,
                            UserID = pp.UserID,
                            UserName = pp.FirstName + " " + pp.LastName
                        };
            return query.FirstOrDefault();
        }

        public QuestionModel GetQuestionDetailById(string problemId)
        {
            var query = from sp in context.StudentProblems
                            //join spb in context.StudentProblemBids on sp.ProblemID equals spb.ProblemID into sasa
                            //from spbd in sasa.DefaultIfEmpty()
                        join sub in context.Subjects
                       on sp.SubjectID equals sub.SubjectID
                        join pp in context.Users on sp.StudentID equals pp.UserID

                        where (sp.ProblemID == problemId)
                        orderby sp.CreationDate ascending
                        select new QuestionModel
                        {
                            ProblemID = sp.ProblemID,
                            CreationDate = sp.CreationDate,
                            ProblemDescription = sp.Description,
                            Deadline = sp.ExpireDate,
                            HoursNeeded = sp.HoursNeeded,
                            Type = sp.Type,
                            SubjectName = sub.SubjectName,
                            UserID = pp.UserID,
                            UserName = pp.FirstName + " " + pp.LastName
                        };
            return query.FirstOrDefault();
        }

        public List<BidsModel> GetBidsByProblemId(string problemId)
        {
            var query = from spb in context.StudentProblemBids
                        join us in context.Users
                       on spb.UserID equals us.UserID

                        where (spb.ProblemID == problemId)
                        orderby spb.CreationDate ascending
                        select new BidsModel
                        {
                            ProblemId = spb.ProblemID,
                            BidId = spb.BidID,
                            ProposalDescription = spb.Description,
                            UserId = us.UserID,
                            UserProfile = us.Picture,
                            UserName = us.FirstName + " " + us.LastName
                        };
            return query.ToList();
        }

        public List<Message> GetMessagesByBidId(string bidId)
        {
            return context.Messages.Where(u => u.BidID == bidId && u.Status == 1).ToList();
        }

        public List<BidsModel> GetBidsByUserId(string userId)
        {
            var query = from spb in context.StudentProblemBids
                        join us in context.Users
                       on spb.UserID equals us.UserID

                        where (us.UserID == userId)
                        orderby spb.CreationDate ascending
                        select new BidsModel
                        {
                            ProblemId = spb.ProblemID,
                            BidId = spb.BidID,
                            ProposalDescription = spb.Description,
                            UserId = us.UserID,
                            UserProfile = us.Picture,
                            UserName = us.FirstName + " " + us.LastName
                        };
            return query.ToList();
        }

        public List<UserIdName> GetUserByType(int type)
        {
            return context.Users.Where(a => a.UserRole == type).Select(x => new UserIdName
            {
                UserId = x.UserID,
                UserName = x.FirstName /*+ " " + x.LastName*/
            }).ToList();
        }

        public List<SearchModel> GetTutorBySubject(int subjectId)
        {
            var query = from ts in context.TeacherStages
                        join us in context.Users
                       on ts.TeacherID equals us.UserID
                        join td in context.TeacherDetails
                        on us.UserID equals td.TeacherID
                        where (ts.SubjectID == subjectId)
                        select new SearchModel
                        {
                            FirstName = us.FirstName,
                            LastName = us.LastName,
                            Picture = us.Picture,
                            TeacherId = us.UserID,
                            SubjectId = ts.SubjectID,
                            Qualification = td.Qualification,
                            Title = td.University
                        };
            return query.ToList();
        }
    }
}
