﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using siekanski_private_lessons_backend.Data;

#nullable disable

namespace siekanski_private_lessons_backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240213152608_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Homework", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LessonId")
                        .IsUnique();

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Homeworks");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Feedback")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeworkId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HomeworkId");

                    b.ToTable("HomeworkTasks");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTaskContentMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeworkTaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HomeworkTaskId");

                    b.ToTable("HomeworkTaskContentMaterials");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTaskFeedbackMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeworkTaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HomeworkTaskId");

                    b.ToTable("HomeworkTaskFeedbackMaterials");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTaskSolutionMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeworkTaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HomeworkTaskId");

                    b.ToTable("HomeworkTaskSolutionMaterials");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaidStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.LessonMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("LessonMaterials");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.TeacherStudent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeacherStudents");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Homework", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.Lesson", "Lesson")
                        .WithOne("Homework")
                        .HasForeignKey("siekanski_private_lessons_backend.Models.Homework", "LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("siekanski_private_lessons_backend.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("siekanski_private_lessons_backend.Models.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTask", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.Homework", "Homework")
                        .WithMany("Tasks")
                        .HasForeignKey("HomeworkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Homework");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTaskContentMaterial", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.HomeworkTask", "HomeworkTask")
                        .WithMany("HomeworkTaskContentMaterials")
                        .HasForeignKey("HomeworkTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeworkTask");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTaskFeedbackMaterial", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.HomeworkTask", "HomeworkTask")
                        .WithMany("HomeworkTaskFeedbackMaterials")
                        .HasForeignKey("HomeworkTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeworkTask");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTaskSolutionMaterial", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.HomeworkTask", "HomeworkTask")
                        .WithMany("HomeworkTaskSolutionMaterials")
                        .HasForeignKey("HomeworkTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeworkTask");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Lesson", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("siekanski_private_lessons_backend.Models.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.LessonMaterial", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.Lesson", "Lesson")
                        .WithMany("LessonMaterials")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Note", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.Lesson", "Lesson")
                        .WithMany("Notes")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.TeacherStudent", b =>
                {
                    b.HasOne("siekanski_private_lessons_backend.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("siekanski_private_lessons_backend.Models.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Homework", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.HomeworkTask", b =>
                {
                    b.Navigation("HomeworkTaskContentMaterials");

                    b.Navigation("HomeworkTaskFeedbackMaterials");

                    b.Navigation("HomeworkTaskSolutionMaterials");
                });

            modelBuilder.Entity("siekanski_private_lessons_backend.Models.Lesson", b =>
                {
                    b.Navigation("Homework")
                        .IsRequired();

                    b.Navigation("LessonMaterials");

                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
