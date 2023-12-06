using Microsoft.EntityFrameworkCore;

namespace ProducerService.API.Models.Entities
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        // public DbSet<Answer> Answers { get; set; }
        // public DbSet<Exam> Exams { get; set; }
        // public DbSet<Question> Questions { get; set; }
        // public DbSet<QuestionExam> QuestionExams { get; set; }
        public DbSet<TestQuestionUserChoose> TestQuestionUserChooses { get; set; }
        public DbSet<TestUser> TestUsers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // //Answer
            // modelBuilder.Entity<Answer>()
            //     .HasOne<Question>(q => q.Question)
            //     .WithMany(q => q.Answers)
            //     .HasForeignKey(q => q.QuestionId);

            // // QuestionExam
            // modelBuilder.Entity<QuestionExam>().HasKey(qe => new { qe.QuestionId, qe.ExamId });
            // modelBuilder.Entity<QuestionExam>()
            //     .HasOne<Exam>(qe => qe.Exam)
            //     .WithMany(e => e.QuestionExams)
            //     .HasForeignKey(qe => qe.ExamId);
            // modelBuilder.Entity<QuestionExam>()
            //     .HasOne<Question>(qe => qe.Question)
            //     .WithMany(q => q.QuestionExams)
            //     .HasForeignKey(qe => qe.QuestionId);

            // TestQuestionUserChoose
            modelBuilder.Entity<TestQuestionUserChoose>()
                .HasOne<User>(tq => tq.User)
                .WithMany(u => u.TestQuestionUserChooses)
                .HasForeignKey(tq => tq.UserId);

            // TestQuestionUserChoose
            modelBuilder.Entity<TestUser>()
                .HasOne<User>(tu => tu.User)
                .WithMany(u => u.TestUsers)
                .HasForeignKey(tu => tu.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}