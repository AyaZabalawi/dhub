using dhub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using dhub.Users;
using Microsoft.AspNetCore.Identity;


namespace dhub.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<QuestionResponseDetails> QuestionResponseDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AnswerOption>()
                .HasOne(a => a.QuestionResponse)
                .WithMany(qr => qr.SelectedOptions)
                .HasForeignKey(a => a.QuestionResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SurveyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionResponse>()
                .HasOne(qr => qr.SurveyResponse)
                .WithMany(sr => sr.QuestionResponses)
                .HasForeignKey(qr => qr.SurveyResponseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionResponse>()
                .HasOne(qr => qr.Question)
                .WithMany()
                .HasForeignKey(qr => qr.QuestionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Survey>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Survey)
                .HasForeignKey(q => q.SurveyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyResponse>()
                .HasMany(sr => sr.QuestionResponses)
                .WithOne(qr => qr.SurveyResponse)
                .HasForeignKey(qr => qr.SurveyResponseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionResponseDetails>()
                .HasOne(qrd => qrd.Question)
                .WithMany()
                .HasForeignKey(qrd => qrd.QuestionID);

            modelBuilder.Entity<QuestionResponseDetails>()
                .HasOne(qrd => qrd.QuestionResponse)
                .WithMany()
                .HasForeignKey(qrd => qrd.QuestionResponseID);
        }
    }
}
