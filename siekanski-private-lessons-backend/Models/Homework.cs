using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace siekanski_private_lessons_backend.Models
{
    public class Homework
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }

        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        [Required]
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public virtual User Teacher { get; set; }
        public virtual User Student { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<HomeworkTask> Tasks { get; set; } = new List<HomeworkTask>();
    }
}
