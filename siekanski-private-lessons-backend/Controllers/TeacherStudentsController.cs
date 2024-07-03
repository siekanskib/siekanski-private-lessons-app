using Microsoft.AspNetCore.Mvc; 
using siekanski_private_lessons_backend.Data; 
using siekanski_private_lessons_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace siekanski_private_lessons_backend.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class TeacherStudentsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LessonsController> _logger;

        public TeacherStudentsController(
            UserManager<User> userManager,
            ApplicationDbContext context,
            ILogger<LessonsController> logger
        )
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddStudentForTeacher([FromBody] AddTeacherStudentDto addTeacherStudentDto)
        {
            _logger.LogInformation($"Received TeacherId: {addTeacherStudentDto.TeacherId}, StudentId: {addTeacherStudentDto.StudentId}");

            var studentExists = await _context.Users.AnyAsync(u => u.Id == addTeacherStudentDto.StudentId && u.Role == "student");
            if (!studentExists)
            {
                return NotFound("Student o podanym ID nie istnieje.");
            }

            var teacherAlreadyHasStudent = await _context.TeacherStudents.AnyAsync(ts =>ts.TeacherId == addTeacherStudentDto.TeacherId && ts.StudentId == addTeacherStudentDto.StudentId);
            if (teacherAlreadyHasStudent)
            {
                return BadRequest("Ten uczeń już znajduje się w Twojej bazie.");
            }

            var teacherStudent = new TeacherStudent
            {
                TeacherId = addTeacherStudentDto.TeacherId,
                StudentId = addTeacherStudentDto.StudentId
            };
            _context.TeacherStudents.Add(teacherStudent);
            await _context.SaveChangesAsync();

            return Ok(addTeacherStudentDto);
        }

        [HttpGet("teacher/{teacherId}/students")]
        public async Task<ActionResult<List<User>>> GetStudentsForTeacher(string teacherId)
        {
            var students = await _context.TeacherStudents
                .Where(ts => ts.TeacherId == teacherId)
                .Select(ts => ts.Student)
                .ToListAsync();

            return Ok(students);
        }
    }
}
