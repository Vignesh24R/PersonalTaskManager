using PersonalTaskManager.Models;

namespace PersonalTaskManager.Repository
{
    public class InMemoryTaskRepository 
    {
        private readonly List<TaskItem> _taskItems = new();
        private int _nextId = 1;

        public IEnumerable<TaskItem> GetAllTaskItems()
        {
            return _taskItems;
        }

        public TaskItem GetTaskItemByTaskId(int id)
        {
            return _taskItems.FirstOrDefault(t => t.Id == id);
        }

        public TaskItem AddTaskItem(TaskItem taskItem)
        {
            taskItem.Id = _nextId++;
            _taskItems.Add(taskItem);
            return taskItem;
        }

        public bool UpdateTaskItem(TaskItem taskItem)
        {
            var existingTaskItem = GetTaskItemByTaskId(taskItem.Id);
            if (existingTaskItem == null)
            {
                return false;
            }
            existingTaskItem.Title = taskItem.Title;
            existingTaskItem.Description = taskItem.Description;
            existingTaskItem.Category = taskItem.Category;
            existingTaskItem.Status = taskItem.Status;
            existingTaskItem.Priority = taskItem.Priority;
            existingTaskItem.DueDate = taskItem.DueDate;
            existingTaskItem.LastModifiedDate = DateTime.UtcNow;
            return true;
        }

        public bool DeleteTaskItem(int id)
        {
            var taskItem = GetTaskItemByTaskId(id);
            if (taskItem == null) return false;
            return _taskItems.Remove(taskItem);
        }
    }
}
