using System.ComponentModel.DataAnnotations;

namespace siekanski_private_lessons_backend.Models
{
    public class HomeworkTaskDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int HomeworkId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Feedback { get; set; }

    }
}
