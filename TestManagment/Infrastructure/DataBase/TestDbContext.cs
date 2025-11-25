using Microsoft.EntityFrameworkCore;
using TestManagment.Domain.Entities;
using TestManagment.Domain.ValueObjects.Question;
using TestManagment.Domain.ValueObjects.Test;

namespace TestManagment.Infrastructure.DataBase
{
    public class TestDbContext: DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestsScheduling> TestsScheduling { get; set; }
        public DbSet<TestQuestion> TestsQuestions { get; set; }

        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>()
                .HasKey(q => q.Id);

            modelBuilder.Entity<Question>()
                .Property(q => q.QuestionText)
                .HasConversion(v => v.Value, v => new QuestionTxt(v))
                .HasColumnName("QuestionText");

            modelBuilder.Entity<Question>()
                .Property(q => q.Choise1)
                .HasConversion(ch => ch.Value, v => new QuestionChoise(v))
                .HasColumnName("Choise1");

            modelBuilder.Entity<Question>()
                .Property(q => q.Choise2)
                .HasConversion(ch => ch.Value, v => new QuestionChoise(v))
                .HasColumnName("Choise2");

            modelBuilder.Entity<Question>()
                .Property(q => q.Choise3)
                .HasConversion(ch => ch.Value, v => new QuestionChoise(v))
                .HasColumnName("Choise3");

            modelBuilder.Entity<Question>()
                .Property(q => q.Answer)
                .HasConversion(ans => ans.Value, v => new QuestionAnswer(v))
                .HasColumnName("Answer");
                

            modelBuilder.Entity<Test>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Test>()
                .Property(t => t.Title)
                .HasConversion(title => title.Value, v => new TestTitle(v))
                .HasColumnName("Title");

            modelBuilder.Entity<Test>()
                .Property(t => t.PublicationStatus)
                .HasConversion(status => status.IsPublished, isPublished => new TestPublicationStatus(isPublished))
                .HasColumnName("IsPublished");

            modelBuilder.Entity<Test>()
                .Property(t => t.IsNotified)
                .HasColumnName("IsNotified")
                .HasDefaultValue(false);

            modelBuilder.Entity<TestsScheduling>()
                .HasKey(t => new {t.TestId, t.DateTime});

            modelBuilder.Entity<TestQuestion>()
                .HasKey(t => new { t.TestId, t.QuestionId });

            modelBuilder.Entity<Test>()
                .HasMany(t => t.TestQuestions)
                .WithOne(tq => tq.Test)
                .HasForeignKey(tq=>tq.TestId);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.TestQuestions)
                .WithOne(tq => tq.question)
                .HasForeignKey(tq=>tq.QuestionId);

            modelBuilder.Entity<Test>()
                .HasOne(t => t.Schedulings)
                .WithOne(s => s.test)
                .HasForeignKey<TestsScheduling>(ts => ts.TestId);

            modelBuilder.UseHiLo();

        }
    }
}
