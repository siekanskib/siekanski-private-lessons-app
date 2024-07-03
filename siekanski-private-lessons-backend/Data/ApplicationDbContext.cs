using Microsoft.EntityFrameworkCore;
using siekanski_private_lessons_backend.Models;

namespace siekanski_private_lessons_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonMaterial> LessonMaterials { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<TeacherStudent> TeacherStudents { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<HomeworkTask> HomeworkTasks { get; set; }
        public DbSet<HomeworkTaskContentMaterial> HomeworkTaskContentMaterials { get; set; }
        public DbSet<HomeworkTaskSolutionMaterial> HomeworkTaskSolutionMaterials { get; set; }
        public DbSet<HomeworkTaskFeedbackMaterial> HomeworkTaskFeedbackMaterials { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<TeacherStudent>()
                .HasOne(ts => ts.Teacher) 
                .WithMany() 
                .HasForeignKey(ts => ts.TeacherId) 
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder
                .Entity<Lesson>()
                .HasOne(l => l.Student) 
                .WithMany() 
                .HasForeignKey(l => l.StudentId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Lesson>()
                .HasOne(l => l.Teacher) 
                .WithMany() 
                .HasForeignKey(l => l.TeacherId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Student) 
                .WithMany() 
                .HasForeignKey(h => h.StudentId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Teacher) 
                .WithMany() 
                .HasForeignKey(h => h.TeacherId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<TeacherStudent>()
                .HasOne(ts => ts.Student) 
                .WithMany() 
                .HasForeignKey(ts => ts.StudentId) 
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Lesson) 
                .WithOne(l => l.Homework) 
                .HasForeignKey<Homework>(h => h.LessonId); 

            modelBuilder
                .Entity<Lesson>()
                .HasOne(l => l.Homework) 
                .WithOne(h => h.Lesson) 
                .HasForeignKey<Homework>(h => h.LessonId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder
                .Entity<Note>()
                .HasOne(n => n.Lesson) 
                .WithMany(l => l.Notes) 
                .HasForeignKey(n => n.LessonId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder
                .Entity<LessonMaterial>()
                .HasOne(lm => lm.Lesson) 
                .WithMany(l => l.LessonMaterials) 
                .HasForeignKey(lm => lm.LessonId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder
                .Entity<Homework>()
                .HasMany(h => h.Tasks) 
                .WithOne(t => t.Homework) 
                .HasForeignKey(t => t.HomeworkId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder
                .Entity<HomeworkTaskContentMaterial>()
                .HasOne(htcm => htcm.HomeworkTask) 
                .WithMany(ht => ht.HomeworkTaskContentMaterials) 
                .HasForeignKey(htcm => htcm.HomeworkTaskId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder
                .Entity<HomeworkTaskSolutionMaterial>()
                .HasOne(htsm => htsm.HomeworkTask) 
                .WithMany(ht => ht.HomeworkTaskSolutionMaterials) 
                .HasForeignKey(htsm => htsm.HomeworkTaskId) 
                .OnDelete(DeleteBehavior.Cascade); 
                
            modelBuilder
                .Entity<HomeworkTaskFeedbackMaterial>()
                .HasOne(htfm => htfm.HomeworkTask) 
                .WithMany(ht => ht.HomeworkTaskFeedbackMaterials) 
                .HasForeignKey(htfm => htfm.HomeworkTaskId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
