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
    public class HomeworkTasksController : ControllerBase 
    {
        private readonly BlobService _blobService;
        private readonly AzureStorageConfig _config;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeworksController> _logger;

        public HomeworkTasksController(
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

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddHomeworkTask([FromBody] HomeworkTaskDTO taskModel)
        {
            _logger.LogInformation($"wszedłem do endpointa tasks");
            var homeworkTask = new HomeworkTask
            {
                HomeworkId = taskModel.HomeworkId,
                Name = taskModel.Name,
                Content = taskModel.Content,
                Feedback = taskModel.Feedback
            };

            _context.HomeworkTasks.Add(homeworkTask);
            await _context.SaveChangesAsync();

            return Ok(new { taskId = homeworkTask.Id });
        }

        [HttpGet("{homeworkId}/tasks")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<HomeworkTask>>> GetTasksForHomework(
            int homeworkId
        )
        {
            var tasks = await _context.HomeworkTasks
                .Where(t => t.HomeworkId == homeworkId)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateHomeworkTask(int id, HomeworkTaskDTO updatedTaskDTO)
        {
            if (id != updatedTaskDTO.Id)
            {
                return BadRequest();
            }

            var homeworkTask = await _context.HomeworkTasks.FindAsync(id);
            if (homeworkTask == null)
            {
                return NotFound();
            }

            homeworkTask.Name = updatedTaskDTO.Name;
            homeworkTask.Content = updatedTaskDTO.Content;
            homeworkTask.Feedback = updatedTaskDTO.Feedback;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool HomeworkTaskExists(int id)
        {
            return _context.HomeworkTasks.Any(e => e.Id == id);
        }

        [HttpGet("{homeworkTaskId}/solutions")]
        [Authorize]
        public async Task<IActionResult> GetSolutionsUrls(int homeworkTaskId)
        {
            var homeworkTask = await _context.HomeworkTasks.FindAsync(homeworkTaskId);

            if (homeworkTask == null)
            {
                return Forbid("Zadanie domowe nie istnieje.");
            }

            var solutions = await _context.HomeworkTaskSolutionMaterials
                .Where(s => s.HomeworkTaskId == homeworkTaskId)
                .ToListAsync();

            if (solutions == null || !solutions.Any())
            {
                return NotFound("Brak rozwiązań dla tego zadania domowego.");
            }

            var solutionsUrls = new List<string>();
            foreach (var solution in solutions)
            {
                var sasUrl = _blobService.GenerateSasUrl(solution.FileName); 
                solutionsUrls.Add(sasUrl);
            }

            return Ok(solutionsUrls);
        }

        [HttpGet("{homeworkTaskId}/feedbacks")]
        [Authorize]
        public async Task<IActionResult> GetFeedbacksUrls(int homeworkTaskId)
        {
            var homeworkTask = await _context.HomeworkTasks.FindAsync(homeworkTaskId);

            if (homeworkTask == null)
            {
                return Forbid("Zadanie domowe nie istnieje.");
            }

            var feedbacks = await _context.HomeworkTaskFeedbackMaterials
                .Where(f => f.HomeworkTaskId == homeworkTaskId)
                .ToListAsync();

            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("Brak feedbackow dla tego zadania domowego.");
            }

            var feedbacksUrls = new List<string>();
            foreach (var feedback in feedbacks)
            {
                var sasUrl = _blobService.GenerateSasUrl(feedback.FileName); 
                feedbacksUrls.Add(sasUrl);
            }

            return Ok(feedbacksUrls);
        }

        [HttpGet("{homeworkTaskId}/contents")]
        [Authorize]
        public async Task<IActionResult> GetContentsUrls(int homeworkTaskId)
        {
            var homeworkTask = await _context.HomeworkTasks.FindAsync(homeworkTaskId);

            if (homeworkTask == null)
            {
                return Forbid("Zadanie domowe nie istnieje.");
            }

            var contents = await _context.HomeworkTaskContentMaterials
                .Where(c => c.HomeworkTaskId == homeworkTaskId)
                .ToListAsync();

            if (contents == null || !contents.Any())
            {
                return NotFound("Brak treści dla tego zadania domowego.");
            }

            var contentsUrls = new List<string>();
            foreach (var content in contents)
            {
                var sasUrl = _blobService.GenerateSasUrl(content.FileName); 
                contentsUrls.Add(sasUrl);
            }

            return Ok(contentsUrls);
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

        [HttpPost("{homeworkTaskId}/upload-solution")]
        [Authorize]
        public async Task<IActionResult> UploadSolution(int homeworkTaskId, IList<IFormFile> files)
        {
            var homeworkTask = await _context.HomeworkTasks.FindAsync(homeworkTaskId);

            if (homeworkTask == null)
            {
                return Forbid("Zadanie domowe nie istnieje.");
            }

            List<object> uploadedSolutions = new List<object>();

            foreach (var solutionFile in files)
            {
                var fileExtension = GetExtensionFromMimeType(solutionFile.ContentType);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                await _blobService.UploadFileToStorage(
                    solutionFile.OpenReadStream(),
                    fileName,
                    solutionFile.ContentType
                );

                var solution = new HomeworkTaskSolutionMaterial { HomeworkTaskId = homeworkTaskId, FileName = fileName, };

                _context.HomeworkTaskSolutionMaterials.Add(solution);
                await _context.SaveChangesAsync();

                uploadedSolutions.Add(new { solutionId = solution.Id, fileName = solution.FileName });
            }

            return Ok(uploadedSolutions);
        }

        [HttpPost("{homeworkTaskId}/upload-feedback")]
        [Authorize]
        public async Task<IActionResult> UploadFeedback(int homeworkTaskId, IList<IFormFile> files)
        {
            var homeworkTask = await _context.HomeworkTasks.FindAsync(homeworkTaskId);

            if (homeworkTask == null)
            {
                return Forbid("Zadanie domowe nie istnieje.");
            }

            List<object> uploadedFeedbacks = new List<object>();

            foreach (var feedbackFile in files)
            {
                var fileExtension = GetExtensionFromMimeType(feedbackFile.ContentType);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                await _blobService.UploadFileToStorage(
                    feedbackFile.OpenReadStream(),
                    fileName,
                    feedbackFile.ContentType
                );

                var feedback = new HomeworkTaskFeedbackMaterial { HomeworkTaskId = homeworkTaskId, FileName = fileName, };

                _context.HomeworkTaskFeedbackMaterials.Add(feedback);
                await _context.SaveChangesAsync();

                uploadedFeedbacks.Add(new { solutionId = feedback.Id, fileName = feedback.FileName });
            }

            return Ok(uploadedFeedbacks);
        }

        [HttpPost("{homeworkTaskId}/upload-content")]
        [Authorize]
        public async Task<IActionResult> UploadContent(int homeworkTaskId, IList<IFormFile> files)
        {
            var homeworkTask = await _context.HomeworkTasks.FindAsync(homeworkTaskId);

            if (homeworkTask == null)
            {
                return Forbid("Zadanie domowe nie istnieje.");
            }

            List<object> uploadedContents = new List<object>();

            foreach (var contentFile in files)
            {
                var fileExtension = GetExtensionFromMimeType(contentFile.ContentType);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                await _blobService.UploadFileToStorage(
                    contentFile.OpenReadStream(),
                    fileName,
                    contentFile.ContentType
                );

                var content = new HomeworkTaskContentMaterial { HomeworkTaskId = homeworkTaskId, FileName = fileName, };

                _context.HomeworkTaskContentMaterials.Add(content);
                await _context.SaveChangesAsync();

                uploadedContents.Add(new { solutionId = content.Id, fileName = content.FileName });
            }

            return Ok(uploadedContents);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.HomeworkTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.HomeworkTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

       
    }
}
