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
            int userId,
            string? status,
            string? category,
            string? search,
            int pageNumber,
            int pageSize)
        {
            return _taskRepository.GetAllTaskItemsAsync(
                userId, status, category, search, pageNumber, pageSize);
        }

        public Task<TaskItem?> GetTaskItemByTaskIdAsync(int id, int userId)
        {
            return _taskRepository.GetTaskItemByTaskIdAsync(id, userId);
        }

        public Task<TaskItem> AddTaskItemAsync(TaskItem taskItem)
        {
            // Could add extra business rules here (e.g., default status)
            return _taskRepository.AddTaskItemAsync(taskItem);
        }

        public Task<bool> UpdateTaskItemAsync(TaskItem taskItem)
        {
            return _taskRepository.UpdateTaskItemAsync(taskItem);
        }

        public Task<bool> DeleteTaskItemAsync(int id, int userId)
        {
            return _taskRepository.DeleteTaskItemAsync(id, userId);
        }
    }
}
