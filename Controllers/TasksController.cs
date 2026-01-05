using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Models;
using PersonalTaskManager.Services;

namespace PersonalTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
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
            var tasks = await _taskService.GetAllTaskItemsAsync(
                status,
                category,
                search,
                pageNumber,
                pageSize);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskItemByTaskId(int id)
        {
            var task = await _taskService.GetTaskItemByTaskIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskItem(TaskItem taskItem)
        {
            var createdTask = await _taskService.AddTaskItemAsync(taskItem);

            return CreatedAtAction(nameof(GetTaskItemByTaskId),
                new { id = createdTask.Id },
                createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskItem(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            var result = await _taskService.UpdateTaskItemAsync(taskItem);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Task updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var result = await _taskService.DeleteTaskItemAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
