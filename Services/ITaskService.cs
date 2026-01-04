using PersonalTaskManager.Models;

namespace PersonalTaskManager.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAllTaskItems();

        TaskItem GetTaskItemByTaskId(int id);

        TaskItem AddTaskItem(TaskItem taskItem);
        bool UpdateTaskItem(TaskItem taskItem);
        bool DeleteTaskItem(int id);
    }
}
