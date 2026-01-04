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
        public IEnumerable<TaskItem> GetAllTaskItems()
        {
            return _taskRepository.GetAllTaskItems();
        }
        public TaskItem GetTaskItemByTaskId(int id)
        {
            return _taskRepository.GetTaskItemByTaskId(id);
        }
        public TaskItem AddTaskItem(TaskItem taskItem)
        {
            return _taskRepository.AddTaskItem(taskItem);
        }
        public bool UpdateTaskItem(TaskItem taskItem)
        {
            return _taskRepository.UpdateTaskItem(taskItem);
        }
        public bool DeleteTaskItem(int id)
        {
            return _taskRepository.DeleteTaskItem(id);
        }
    }
}
