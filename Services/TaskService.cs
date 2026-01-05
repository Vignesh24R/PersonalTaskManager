using PersonalTaskManager.Models;
using PersonalTaskManager.Repository;

namespace PersonalTaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync(
            string? status,
            string? category,
            string? search,
            int pageNumber,
            int pageSize)
        {
            return _taskRepository.GetAllTaskItemsAsync(status, category, search, pageNumber, pageSize);
        }

        public Task<TaskItem?> GetTaskItemByTaskIdAsync(int id)
        {
            return _taskRepository.GetTaskItemByTaskIdAsync(id);
        }

        public Task<TaskItem> AddTaskItemAsync(TaskItem taskItem)
        {
            return _taskRepository.AddTaskItemAsync(taskItem);
        }

        public Task<bool> UpdateTaskItemAsync(TaskItem taskItem)
        {
            return _taskRepository.UpdateTaskItemAsync(taskItem);
        }

        public Task<bool> DeleteTaskItemAsync(int id)
        {
            return _taskRepository.DeleteTaskItemAsync(id);
        }
    }
}
