using PersonalTaskManager.Models;

namespace PersonalTaskManager.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetAllTaskItems();

        TaskItem GetTaskItemByTaskId(int id);

        TaskItem AddTaskItem(TaskItem taskItem);
        bool UpdateTaskItem(TaskItem taskItem);
        bool DeleteTaskItem(int id);
        //IEnumerable<TaskItem> GetTaskItemsByUserId(int userId);
    }
}
