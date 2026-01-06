using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Models;
using PersonalTaskManager.Services;
using System.Security.Claims;

namespace PersonalTaskManager.Controllers
{
    [Authorize]  // require JWT for all endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Helper: get current user ID from JWT
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User id claim missing");
            }

            return int.Parse(userIdClaim.Value);
        }

        // GET /api/tasks?status=&category=&search=&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAllTasks(
            [FromQuery] string? status,
            [FromQuery] string? category,
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = GetCurrentUserId();

            var tasks = await _taskService.GetAllTaskItemsAsync(
                userId, status, category, search, pageNumber, pageSize);

            return Ok(tasks);
        }

        // GET /api/tasks/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskItemByTaskId(int id)
        {
            var userId = GetCurrentUserId();

            var task = await _taskService.GetTaskItemByTaskIdAsync(id, userId);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // POST /api/tasks
        [HttpPost]
        public async Task<IActionResult> AddTaskItem([FromBody] TaskItem taskItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            taskItem.UserId = userId;  // enforce owner

            var createdTask = await _taskService.AddTaskItemAsync(taskItem);

            return CreatedAtAction(
                nameof(GetTaskItemByTaskId),
                new { id = createdTask.Id },
                createdTask);
        }

        // PUT /api/tasks/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTaskItem(int id, [FromBody] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest("Route id and body id must match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            taskItem.UserId = userId;  // enforce owner

            var success = await _taskService.UpdateTaskItemAsync(taskItem);
            if (!success)
            {
                return NotFound();  // not found or not owned
            }

            return Ok(new { message = "Task updated successfully" });
        }

        // DELETE /api/tasks/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var userId = GetCurrentUserId();

            var success = await _taskService.DeleteTaskItemAsync(id, userId);
            if (!success)
            {
                return NotFound();
            }

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
