using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskService.GetAllTaskItems();
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public IActionResult GetTaskItemByTaskId(int id)
        {
            var task = _taskService.GetTaskItemByTaskId(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        [HttpPost]
        public IActionResult AddTaskItem(TaskItem taskItem)
        {
            var createdTask = _taskService.AddTaskItem(taskItem);

            return CreatedAtAction(nameof(GetTaskItemByTaskId), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTaskItem(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }
            var result = _taskService.UpdateTaskItem(taskItem);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTaskItem(int id)
        {
            var result = _taskService.DeleteTaskItem(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
