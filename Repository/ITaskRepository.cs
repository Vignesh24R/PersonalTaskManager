using PersonalTaskManager.Models;

namespace PersonalTaskManager.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync(
            string? status,
            string? category,
            string? search,
            int pageNumber,
            int pageSize);

        Task<TaskItem?> GetTaskItemByTaskIdAsync(int id);

        Task<TaskItem> AddTaskItemAsync(TaskItem taskItem);
        Task<bool> UpdateTaskItemAsync(TaskItem taskItem);
        Task<bool> DeleteTaskItemAsync(int id);
    }
}
