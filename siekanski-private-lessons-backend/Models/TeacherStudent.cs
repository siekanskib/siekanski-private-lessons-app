using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace siekanski_private_lessons_backend.Models
{
    public class TeacherStudent
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }

        public virtual User Teacher { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public virtual User Student { get; set; }
    }
}

