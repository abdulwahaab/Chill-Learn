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

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassInvitation> ClassInvitations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Refund> Refunds { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<StudentProblemBid> StudentProblemBids { get; set; }
        public virtual DbSet<StudentProblem> StudentProblems { get; set; }
        public virtual DbSet<SubjectPrice> SubjectPrices { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<TeacherDetail> TeacherDetails { get; set; }
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

            modelBuilder.Entity<Class>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Plan>()
                .Property(e => e.Price)
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

            modelBuilder.Entity<TeacherReview>()
                .Property(e => e.Hours)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TeacherStage>()
                .Property(e => e.HourlyRate)
                .HasPrecision(18, 0);
        }
    }
}
