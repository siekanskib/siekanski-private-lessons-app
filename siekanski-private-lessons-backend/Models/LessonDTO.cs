using System.ComponentModel.DataAnnotations;

namespace siekanski_private_lessons_backend.Models
{
    public class LessonCreateDTO
    {
        [Required]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PaidStatus { get; set; }
        public string TeacherId { get; set; }
        public string StudentId { get; set; }
    }

    public class LessonUpdateDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PaidStatus { get; set; }
    }
}
