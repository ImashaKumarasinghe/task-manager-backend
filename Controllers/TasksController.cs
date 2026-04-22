using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_task_manager_backend.Data;
using mini_task_manager_backend.DTOs;
using mini_task_manager_backend.Models;
using mini_task_manager_backend.Services;

namespace mini_task_manager_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FirebaseTokenService _firebaseTokenService;

        public TasksController(AppDbContext context, FirebaseTokenService firebaseTokenService)
        {
            _context = context;
            _firebaseTokenService = firebaseTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();

                if (userId == null)
                    return Unauthorized("Invalid or missing token");

                var tasks = await _context.Tasks
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                return Ok(tasks);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();

                if (userId == null)
                    return Unauthorized("Invalid or missing token");

                if (dto == null)
                    return BadRequest("Request body is required");

                if (string.IsNullOrWhiteSpace(dto.Title))
                    return BadRequest("Task title is required");

                var task = new TaskItem
                {
                    Title = dto.Title.Trim(),
                    Description = dto.Description,
                    IsCompleted = false,
                    UserId = userId
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return Ok(task);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();

                if (userId == null)
                    return Unauthorized("Invalid or missing token");

                if (dto == null)
                    return BadRequest("Request body is required");

                if (string.IsNullOrWhiteSpace(dto.Title))
                    return BadRequest("Task title is required");

                var task = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (task == null)
                    return NotFound("Task not found");

                task.Title = dto.Title.Trim();
                task.Description = dto.Description;
                task.IsCompleted = dto.IsCompleted;

                await _context.SaveChangesAsync();

                return Ok(task);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();

                if (userId == null)
                    return Unauthorized("Invalid or missing token");

                var task = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (task == null)
                    return NotFound("Task not found");

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Task deleted successfully" });
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        private async Task<string?> GetCurrentUserIdAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            return await _firebaseTokenService.VerifyTokenAsync(authHeader);
        }
    }
}