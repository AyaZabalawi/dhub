﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dhub.Data;

#nullable disable

namespace dhub.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240821080528_change2")]
    partial class change2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("dhub.Models.AnswerOption", b =>
                {
                    b.Property<Guid>("AnswerOptionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OptionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionResponseID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AnswerOptionID");

                    b.HasIndex("QuestionResponseID");

                    b.ToTable("AnswerOptions");
                });

            modelBuilder.Entity("dhub.Models.Question", b =>
                {
                    b.Property<Guid>("QuestionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<string>("Options")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionType")
                        .HasColumnType("int");

                    b.Property<Guid>("SurveyID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionID");

                    b.HasIndex("SurveyID");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("dhub.Models.QuestionResponse", b =>
                {
                    b.Property<Guid>("QuestionResponseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SurveyResponseID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionResponseID");

                    b.HasIndex("QuestionID");

                    b.HasIndex("SurveyResponseID");

                    b.ToTable("QuestionResponses");
                });

            modelBuilder.Entity("dhub.Models.Survey", b =>
                {
                    b.Property<Guid>("SurveyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SurveyID");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("dhub.Models.SurveyResponse", b =>
                {
                    b.Property<Guid>("ResponseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SurveyID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ResponseID");

                    b.HasIndex("SurveyID");

                    b.ToTable("SurveyResponses");
                });

            modelBuilder.Entity("dhub.Models.AnswerOption", b =>
                {
                    b.HasOne("dhub.Models.QuestionResponse", "QuestionResponse")
                        .WithMany("SelectedOptions")
                        .HasForeignKey("QuestionResponseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuestionResponse");
                });

            modelBuilder.Entity("dhub.Models.Question", b =>
                {
                    b.HasOne("dhub.Models.Survey", "Survey")
                        .WithMany("Questions")
                        .HasForeignKey("SurveyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("dhub.Models.QuestionResponse", b =>
                {
                    b.HasOne("dhub.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("dhub.Models.SurveyResponse", "SurveyResponse")
                        .WithMany("QuestionResponses")
                        .HasForeignKey("SurveyResponseID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("SurveyResponse");
                });

            modelBuilder.Entity("dhub.Models.SurveyResponse", b =>
                {
                    b.HasOne("dhub.Models.Survey", "Survey")
                        .WithMany()
                        .HasForeignKey("SurveyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("dhub.Models.QuestionResponse", b =>
                {
                    b.Navigation("SelectedOptions");
                });

            modelBuilder.Entity("dhub.Models.Survey", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("dhub.Models.SurveyResponse", b =>
                {
                    b.Navigation("QuestionResponses");
                });
#pragma warning restore 612, 618
        }
    }
}
