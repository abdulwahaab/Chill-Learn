using System;
using ChillLearn.Data.Models;

namespace ChillLearn.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ChillLearnContext context = new ChillLearnContext();

        private Repository<AppSetting> appRepository;
        private Repository<Claim> claimRepository;
        private Repository<Class> classRepository;
        private Repository<ClassInvitation> classInvitationRepository;
        private Repository<Message> messageRepository;
        private Repository<Payment> paymentRepository;
        private Repository<Plan> planRepository;
        private Repository<Refund> refundRepository;
        private Repository<Stage> stageRepository;
        private Repository<StudentCredit> studentCreditRepository;
        private Repository<StudentCreditLog> studentCreditLogRepository;
        private Repository<StudentClass> studentClassRepository;
        private Repository<StudentProblem> studentProblemRepository;
        private Repository<StudentProblemBid> studentProblemBidRepository;
        private Repository<Subject> subjectRepository;
        private Repository<SubjectPrice> subjectPriceRepository;
        private Repository<Subscription> subscriptionRepository;
        private Repository<TeacherCertification> teacherCertificationRepository;
        private Repository<TeacherDetail> teacherDetailRepository;
        private Repository<TeacherQualification> teacherQualificationRepository;
        private Repository<TeacherReview> teacherReviewRepository;
        private Repository<TeacherStage> teacherStageRepository;
        private Repository<User> userRepository;
        private Repository<UserClaim> userClaimRepository;
        private Repository<Wallet> walletRepository;
        private Repository<ClassFile> classFileRepository;
        private Repository<Country> countryRepository;
        private Repository<TeacherAccountDetail> teacherAccountDetailRepository;

        //custom repositories
        private UserRepository User;
        private TeacherRepository Teacher;
        private StudentRepository Student;


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Repository<AppSetting> AppSettings => appRepository ?? (appRepository = new Repository<AppSetting>(context));
        public Repository<Claim> Claims => claimRepository ?? (claimRepository = new Repository<Claim>(context));
        public Repository<Class> Classes => classRepository ?? (classRepository = new Repository<Class>(context));
        public Repository<ClassInvitation> ClassInvitations => classInvitationRepository ?? (classInvitationRepository = new Repository<ClassInvitation>(context));
        public Repository<Message> Messages => messageRepository ?? (messageRepository = new Repository<Message>(context));
        public Repository<Payment> Payments => paymentRepository ?? (paymentRepository = new Repository<Payment>(context));
        public Repository<Plan> Plans => planRepository ?? (planRepository = new Repository<Plan>(context));
        public Repository<Refund> Refunds => refundRepository ?? (refundRepository = new Repository<Refund>(context));
        public Repository<Stage> Stages => stageRepository ?? (stageRepository = new Repository<Stage>(context));
        public Repository<StudentCredit> StudentCredits => studentCreditRepository ?? (studentCreditRepository = new Repository<StudentCredit>(context));
        public Repository<StudentCreditLog> StudentCreditLogs => studentCreditLogRepository ?? (studentCreditLogRepository = new Repository<StudentCreditLog>(context));
        public Repository<StudentClass> StudentClasses => studentClassRepository ?? (studentClassRepository = new Repository<StudentClass>(context));
        public Repository<StudentProblem> StudentProblems => studentProblemRepository ?? (studentProblemRepository = new Repository<StudentProblem>(context));
        public Repository<StudentProblemBid> StudentProblemBids => studentProblemBidRepository ?? (studentProblemBidRepository = new Repository<StudentProblemBid>(context));
        public Repository<Subject> Subjects => subjectRepository ?? (subjectRepository = new Repository<Subject>(context));
        public Repository<SubjectPrice> SubjectPrices => subjectPriceRepository ?? (subjectPriceRepository = new Repository<SubjectPrice>(context));
        public Repository<Subscription> Subscriptions => subscriptionRepository ?? (subscriptionRepository = new Repository<Subscription>(context));
        public Repository<TeacherCertification> TeacherCertifications => teacherCertificationRepository ?? (teacherCertificationRepository = new Repository<TeacherCertification>(context));
        public Repository<TeacherDetail> TeacherDetails => teacherDetailRepository ?? (teacherDetailRepository = new Repository<TeacherDetail>(context));
        public Repository<TeacherQualification> TeacherQualifications => teacherQualificationRepository ?? (teacherQualificationRepository = new Repository<TeacherQualification>(context));
        public Repository<TeacherReview> TeacherReviews => teacherReviewRepository ?? (teacherReviewRepository = new Repository<TeacherReview>(context));
        public Repository<TeacherStage> TeacherStages => teacherStageRepository ?? (teacherStageRepository = new Repository<TeacherStage>(context));
        public Repository<User> Users => userRepository ?? (userRepository = new Repository<User>(context));
        public Repository<UserClaim> UserClaims => userClaimRepository ?? (userClaimRepository = new Repository<UserClaim>(context));
        public Repository<Wallet> Wallets => walletRepository ?? (walletRepository = new Repository<Wallet>(context));
        public Repository<ClassFile> ClassFiles => classFileRepository ?? (classFileRepository = new Repository<ClassFile>(context));
        public Repository<Country> Countries => countryRepository ?? (countryRepository = new Repository<Country>(context));
        public Repository<TeacherAccountDetail> TeacherAccountDetails => teacherAccountDetailRepository ?? (teacherAccountDetailRepository = new Repository<TeacherAccountDetail>(context));
        public UserRepository UserRepository => User ?? (User = new UserRepository(context));
        public TeacherRepository TeacherRepository => Teacher ?? (Teacher = new TeacherRepository(context));
        public StudentRepository StudentRepository => Student ?? (Student = new StudentRepository(context));
    }
}
