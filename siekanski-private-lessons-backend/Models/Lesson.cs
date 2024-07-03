using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace siekanski_private_lessons_backend.Models
{
    public class Lesson
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PaidStatus { get; set; }

        [Required]
        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }

        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
        public virtual ICollection<LessonMaterial> LessonMaterials { get; set; } =
            new List<LessonMaterial>();
        public virtual User Teacher { get; set; }
        public virtual User Student { get; set; }
        public virtual Homework Homework { get; set; }
    }
}
