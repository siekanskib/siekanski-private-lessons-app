using System.ComponentModel.DataAnnotations;

namespace siekanski_private_lessons_backend.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }
        [Required]
        public string FileName { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
