using System.ComponentModel.DataAnnotations;

namespace siekanski_private_lessons_backend.Models
{
    public class HomeworkTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HomeworkId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Feedback { get; set; }

        public virtual Homework Homework { get; set; }

        public virtual ICollection<HomeworkTaskContentMaterial> HomeworkTaskContentMaterials { get; set; } =
            new List<HomeworkTaskContentMaterial>();

        public virtual ICollection<HomeworkTaskSolutionMaterial> HomeworkTaskSolutionMaterials { get; set; } =
            new List<HomeworkTaskSolutionMaterial>();

        public virtual ICollection<HomeworkTaskFeedbackMaterial> HomeworkTaskFeedbackMaterials { get; set; } =
            new List<HomeworkTaskFeedbackMaterial>();
    }
}
