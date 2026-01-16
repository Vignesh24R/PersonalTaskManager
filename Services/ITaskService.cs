using PersonalTaskManager.Models;

namespace PersonalTaskManager.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync(
            int userId,
            string? status,
            string? category,
            string? search,
            int pageNumber,
            int pageSize);

        Task<TaskItem?> GetTaskItemByTaskIdAsync(int id, int userId);

        Task<TaskItem> AddTaskItemAsync(TaskItem taskItem);

        Task<bool> UpdateTaskItemAsync(TaskItem taskItem);

        Task<bool> DeleteTaskItemAsync(int id, int userId);
    }
}
