namespace ChillLearn.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ChillLearnContext : DbContext
    {
        public ChillLearnContext()
            : base("name=ChillLearnContext")
        {
        }

        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassFile> ClassFiles { get; set; }
        public virtual DbSet<ClassInvitation> ClassInvitations { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Refund> Refunds { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<StudentCreditLog> StudentCreditLogs { get; set; }
        public virtual DbSet<StudentCredit> StudentCredits { get; set; }
        public virtual DbSet<StudentProblemBid> StudentProblemBids { get; set; }
        public virtual DbSet<StudentProblemFile> StudentProblemFiles { get; set; }
        public virtual DbSet<StudentProblem> StudentProblems { get; set; }
        public virtual DbSet<SubjectPrice> SubjectPrices { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<TeacherAccountDetail> TeacherAccountDetails { get; set; }
        public virtual DbSet<TeacherCertification> TeacherCertifications { get; set; }
        public virtual DbSet<TeacherCreditLog> TeacherCreditLogs { get; set; }
        public virtual DbSet<TeacherDetail> TeacherDetails { get; set; }
        public virtual DbSet<TeacherFile> TeacherFiles { get; set; }
        public virtual DbSet<TimeZones> TimeZones { get; set; }
        public virtual DbSet<TeacherLanguage> TeacherLanguages { get; set; }
        public virtual DbSet<TeacherQualification> TeacherQualifications { get; set; }
        public virtual DbSet<TeacherReview> TeacherReviews { get; set; }
        public virtual DbSet<TeacherStage> TeacherStages { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.FeaturedClassPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.FeaturedTeacherPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Country>()
                .Property(e => e.Iso)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.Iso3)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Plan>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Plan>()
                .Property(e => e.Hours)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Refund>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StudentProblem>()
                .Property(e => e.HoursNeeded)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StudentProblem>()
                .Property(e => e.HoursSpent)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SubjectPrice>()
                .Property(e => e.HourlyRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SubjectPrice>()
                .Property(e => e.TeacherShare)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.Hours)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TeacherReview>()
                .Property(e => e.Hours)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TeacherStage>()
                .Property(e => e.HourlyRate)
                .HasPrecision(18, 0);
        }
    }
}
