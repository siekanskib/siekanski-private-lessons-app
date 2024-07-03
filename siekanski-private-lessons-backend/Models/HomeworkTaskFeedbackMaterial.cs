using System.ComponentModel.DataAnnotations;

namespace siekanski_private_lessons_backend.Models
{
    public class HomeworkTaskFeedbackMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HomeworkTaskId { get; set; }
        [Required]
        public string FileName { get; set; }

        public virtual HomeworkTask HomeworkTask { get; set; }
    }
}
