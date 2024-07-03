using Microsoft.AspNetCore.Mvc; // Importowanie przestrzeni nazw do obsługi kontrolerów MVC
using siekanski_private_lessons_backend.Data; // Twoja przestrzeń nazw dla kontekstu bazy danych
using siekanski_private_lessons_backend.Models; // Twoja przestrzeń nazw dla modeli danych
using System.Threading.Tasks; // Przestrzeń nazw potrzebna do pracy z asynchronicznością
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace siekanski_private_lessons_backend.Controllers 
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class LessonsController : ControllerBase 
    {
        private readonly BlobService _blobService;
        private readonly AzureStorageConfig _config;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly ILogger<LessonsController> _logger;

        public LessonsController(
            BlobService blobService,
            IOptions<AzureStorageConfig> config,
            UserManager<User> userManager,
            ApplicationDbContext context,
            ILogger<LessonsController> logger
        )
        {
            _blobService = blobService;
            _config = config.Value;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Lesson>> CreateLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            var homework = new Homework
            {
                LessonId = lesson.Id,
                TeacherId = lesson.TeacherId,
                StudentId = lesson.StudentId,
                Description = $"Praca domowa",
                Status = "Do zrobienia" 
            };


            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetLesson", new { id = lesson.Id }, lesson);
        }

        [HttpGet("forTeacher")]
        [Authorize]
        public async Task<ActionResult> GetLessonsForTeacher(
            [FromQuery] string studentId = null,
            [FromQuery] string isPaid = null,
            [FromQuery] string homework = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6
        )
        {
            var userId = _userManager.GetUserId(User);

            IQueryable<Lesson> query = _context.Lessons.Include(l => l.Homework);

            query = query.Where(l => l.TeacherId == userId);
            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(l => l.StudentId == studentId);
            }
            if (!string.IsNullOrEmpty(isPaid))
            {
                query = query.Where(l => l.PaidStatus == isPaid);
            }
            if (!string.IsNullOrEmpty(homework))
            {
                query = query.Where(l => l.Homework.Status == homework);
            }

            query = query.OrderByDescending(l => l.Date);

            var amountOfLessons = await query.CountAsync();

            var lessons = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(
                new
                {
                    Lessons = lessons,
                    Pagination = new
                    {
                        AmountOfLessons = amountOfLessons,
                        PageSize = pageSize,
                        PageNumber = pageNumber,
                        TotalPages = (int)Math.Ceiling(amountOfLessons / (double)pageSize)
                    }
                }
            );
        }

        [HttpGet("forStudent")]
        [Authorize]
        public async Task<ActionResult> GetLessonsForStudent(
            [FromQuery] string isPaid = null,
            [FromQuery] string homework = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6
        )
        {
            var userId = _userManager.GetUserId(User);

            IQueryable<Lesson> query = _context.Lessons.Include(l => l.Homework);

            query = query.Where(l => l.StudentId == userId);
            if (!string.IsNullOrEmpty(isPaid))
            {
                query = query.Where(l => l.PaidStatus == isPaid);
            }
            if (!string.IsNullOrEmpty(homework))
            {
                query = query.Where(l => l.Homework.Status == homework);
            }

            query = query.OrderByDescending(l => l.Date);

            var amountOfLessons = await query.CountAsync();

            var lessons = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(
                new
                {
                    Lessons = lessons,
                    Pagination = new
                    {
                        AmountOfLessons = amountOfLessons,
                        PageSize = pageSize,
                        PageNumber = pageNumber,
                        TotalPages = (int)Math.Ceiling(amountOfLessons / (double)pageSize)
                    }
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddLesson([FromBody] LessonCreateDTO lessonDTO)
        {
            var lesson = new Lesson
            {
                Date = lessonDTO.Date,
                Name = lessonDTO.Name,
                Description = lessonDTO.Description,
                PaidStatus = lessonDTO.PaidStatus,
                TeacherId = lessonDTO.TeacherId,
                StudentId = lessonDTO.StudentId
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            var homework = new Homework
            {
                LessonId = lesson.Id,
                TeacherId = lesson.TeacherId,
                StudentId = lesson.StudentId,
                Description = $"Praca domowa z lekcji z dnia: {lesson.Date.ToString("yyyy-MM-dd")}",
                Status = "Brak"
            };

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLesson", new { id = lesson.Id }, lesson);
        }

        [HttpGet("{lessonId}/notes")]
        [Authorize]
        public async Task<IActionResult> GetNotesUrls(int lessonId)
        {
            var userId = _userManager.GetUserId(User);
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (
                lesson == null
                || (lesson.StudentId.ToString() != userId && lesson.TeacherId.ToString() != userId)
            )
            {
                return Forbid("Nie masz dostępu do tej lekcji.");
            }

            var notes = await _context.Notes.Where(n => n.LessonId == lessonId).ToListAsync();

            if (notes == null || !notes.Any())
            {
                return NotFound("Brak notatek dla tej lekcji.");
            }

            var notesUrls = new List<string>();
            foreach (var note in notes)
            {
                var sasUrl = _blobService.GenerateSasUrl(note.FileName); 
                notesUrls.Add(sasUrl);
            }

            return Ok(notesUrls);
        }

        [HttpGet("{lessonId}/materials")]
        [Authorize]
        public async Task<IActionResult> GetMaterialsUrls(int lessonId)
        {
            var userId = _userManager.GetUserId(User);
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (
                lesson == null
                || (lesson.StudentId.ToString() != userId && lesson.TeacherId.ToString() != userId)
            )
            {
                return Forbid("Nie masz dostępu do tej lekcji.");
            }

            var materials = await _context.LessonMaterials
                .Where(n => n.LessonId == lessonId)
                .ToListAsync();

            if (materials == null || !materials.Any())
            {
                return NotFound("Brak materiałów dla tej lekcji.");
            }

            var materialsUrls = new List<string>();
            foreach (var material in materials)
            {
                var sasUrl = _blobService.GenerateSasUrl(material.FileName); 
                materialsUrls.Add(sasUrl);
            }

            return Ok(materialsUrls);
        }

        private string GetExtensionFromMimeType(string mimeType)
        {
            var mimeTypes = new Dictionary<string, string>
            {
                { "image/jpeg", ".jpg" },
                { "image/png", ".png" },
                { "application/pdf", ".pdf" },
            };

            if (mimeTypes.TryGetValue(mimeType, out var extension))
            {
                return extension;
            }

            return null;
        }

        [HttpPost("{lessonId}/upload-note")]
        [Authorize]
        public async Task<IActionResult> UploadNote(int lessonId, IList<IFormFile> files)
        {
            var userId = _userManager.GetUserId(User);
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (
                lesson == null
                || (lesson.StudentId.ToString() != userId && lesson.TeacherId.ToString() != userId)
            )
            {
                return Forbid("Nie masz dostępu do tej lekcji.");
            }

            List<object> uploadedNotes = new List<object>();

            foreach (var noteFile in files)
            {
                var fileExtension = GetExtensionFromMimeType(noteFile.ContentType);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                await _blobService.UploadFileToStorage(
                    noteFile.OpenReadStream(),
                    fileName,
                    noteFile.ContentType
                );

                var note = new Note { LessonId = lessonId, FileName = fileName, };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                uploadedNotes.Add(new { noteId = note.Id, fileName = note.FileName });
            }

            return Ok(uploadedNotes);
        }

        [HttpPost("{lessonId}/upload-material")]
        [Authorize]
        public async Task<IActionResult> UploadMaterial(int lessonId, IList<IFormFile> files)
        {
            _logger.LogInformation($"wszedłem do endpointa");
            var userId = _userManager.GetUserId(User);
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (
                lesson == null
                || (lesson.StudentId.ToString() != userId && lesson.TeacherId.ToString() != userId)
            )
            {
                return Forbid("Nie masz dostępu do tej lekcji.");
            }

            List<object> uploadedMaterials = new List<object>();

            foreach (var materialFile in files)
            {
                var fileExtension = GetExtensionFromMimeType(materialFile.ContentType);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                await _blobService.UploadFileToStorage(
                    materialFile.OpenReadStream(),
                    fileName,
                    materialFile.ContentType
                );

                var material = new LessonMaterial { LessonId = lessonId, FileName = fileName, };

                _context.LessonMaterials.Add(material);
                await _context.SaveChangesAsync();

                uploadedMaterials.Add(
                    new { materialId = material.Id, fileName = material.FileName }
                );
            }

            return Ok(uploadedMaterials);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, LessonUpdateDTO updatedLessonDTO)
        {
            if (id != updatedLessonDTO.Id)
            {
                return BadRequest();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound("Nie znaleziono lekcji");
            }

            lesson.Description = updatedLessonDTO.Description;
            lesson.PaidStatus = updatedLessonDTO.PaidStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("Aktualizowania zadania");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
