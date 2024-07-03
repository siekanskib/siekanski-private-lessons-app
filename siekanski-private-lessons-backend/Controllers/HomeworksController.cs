using Microsoft.AspNetCore.Mvc; 
using siekanski_private_lessons_backend.Data; 
using siekanski_private_lessons_backend.Models; 
using System.Threading.Tasks; 
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
    public class HomeworksController : ControllerBase 
    {
        private readonly BlobService _blobService;
        private readonly AzureStorageConfig _config;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeworksController> _logger;

        public HomeworksController(
            BlobService blobService,
            IOptions<AzureStorageConfig> config,
            UserManager<User> userManager,
            ApplicationDbContext context,
            ILogger<HomeworksController> logger
        )
        {
            _blobService = blobService;
            _config = config.Value;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet("by-lesson/{lessonId}")]
        public async Task<ActionResult> GetHomeworkByLessonId(int lessonId)
        {
            var homework = await _context.Homeworks
                .Where(h => h.LessonId == lessonId)
                .Select(h => new { h.Id })
                .FirstOrDefaultAsync();

            if (homework == null)
            {
                return NotFound("Praca domowa dla tej lekcji nie istnieje.");
            }

            return Ok(homework);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Homework>> GetHomework(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);

            if (homework == null)
            {
                return NotFound();
            }

            return homework;
        }

        [HttpPut("{homeworkId}/status")]
        public async Task<IActionResult> UpdateHomeworkStatus(
            int homeworkId,
            [FromBody] HomeworkDTO HomeworkDTO
        )
        {
            var homework = await _context.Homeworks.FindAsync(homeworkId);

            if (homework == null)
            {
                return NotFound();
            }

            homework.Status = HomeworkDTO.Status;
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
