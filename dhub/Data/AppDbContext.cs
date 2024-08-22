using Microsoft.EntityFrameworkCore;
using dhub.Models;
using dhub.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace dhub.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<QuestionResponseDetails> QuestionResponseDetails { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

     
            modelBuilder.Entity<Survey>()
                .HasKey(s => s.SurveyID);

            modelBuilder.Entity<Survey>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Survey)
                .HasForeignKey(q => q.SurveyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Survey>()
                .HasOne(s => s.Owner)
                .WithMany()
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

        
            modelBuilder.Entity<Question>()
                .HasKey(q => q.QuestionID);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SurveyID)
                .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<SurveyResponse>()
                .HasKey(sr => sr.ResponseID);

            modelBuilder.Entity<SurveyResponse>()
                .HasMany(sr => sr.QuestionResponses)
                .WithOne(qr => qr.SurveyResponse)
                .HasForeignKey(qr => qr.SurveyResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(sr => sr.Survey)
                .WithMany()
                .HasForeignKey(sr => sr.SurveyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionResponse>()
                .HasKey(qr => qr.QuestionResponseID);

            modelBuilder.Entity<QuestionResponse>()
                .HasOne(qr => qr.SurveyResponse)
                .WithMany(sr => sr.QuestionResponses)
                .HasForeignKey(qr => qr.SurveyResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionResponse>()
                .HasOne(qr => qr.Question)
                .WithMany()
                .HasForeignKey(qr => qr.QuestionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionResponse>()
                .HasMany(qr => qr.SelectedOptions)
                .WithOne(ao => ao.QuestionResponse)
                .HasForeignKey(ao => ao.QuestionResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionResponseDetails>()
                .HasKey(qrd => qrd.QuestionResponseDetailId);

            modelBuilder.Entity<QuestionResponseDetails>()
                .HasOne(qrd => qrd.QuestionResponse)
                .WithMany()
                .HasForeignKey(qrd => qrd.QuestionResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionResponseDetails>()
                .HasOne(qrd => qrd.Question)
                .WithMany()
                .HasForeignKey(qrd => qrd.QuestionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AnswerOption>()
                .HasKey(ao => ao.AnswerOptionID);

            modelBuilder.Entity<AnswerOption>()
                .HasOne(ao => ao.QuestionResponse)
                .WithMany(qr => qr.SelectedOptions)
                .HasForeignKey(ao => ao.QuestionResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
