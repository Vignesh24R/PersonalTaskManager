using Microsoft.EntityFrameworkCore;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync(
            int userId,
            string? status,
            string? category,
            string? search,
            int pageNumber,
            int pageSize)
        {
            // Start with tasks for this user only
            IQueryable<TaskItem> query = _context.TaskItems
                .Where(t => t.UserId == userId);

            // Optional filters
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(t => t.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    t.Title.Contains(search) ||
                    (t.Description != null && t.Description.Contains(search)));
            }

            // Order + pagination
            query = query.OrderBy(t => t.Id)
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskItemByTaskIdAsync(int id, int userId)
        {
            // Ensure the task belongs to this user
            return await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TaskItem> AddTaskItemAsync(TaskItem taskItem)
        {
            // UserId should already be set by controller/service
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<bool> UpdateTaskItemAsync(TaskItem taskItem)
        {
            // Load existing and check owner
            var existing = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskItem.Id && t.UserId == taskItem.UserId);

            if (existing == null)
            {
                return false; // not found or not owned
            }

            // Update mutable fields
            existing.Title = taskItem.Title;
            existing.Description = taskItem.Description;
            existing.Category = taskItem.Category;
            existing.Status = taskItem.Status;
            existing.Priority = taskItem.Priority;
            existing.DueDate = taskItem.DueDate;
            existing.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskItemAsync(int id, int userId)
        {
            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
            {
                return false;
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
