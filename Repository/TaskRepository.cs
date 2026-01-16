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
            string? status,
            string? category,
            string? search,
            int pageNumber,
            int pageSize)
        {
            // Start as IQueryable so we can apply filters and pagination server-side
            IQueryable<TaskItem> query = _context.TaskItems.AsQueryable();

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
                    t.Description.Contains(search));
            }

            // Basic ordering, then pagination with Skip/Take
            query = query.OrderBy(t => t.Id);

            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskItemByTaskIdAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task<TaskItem> AddTaskItemAsync(TaskItem taskItem)
        {
            // EF will generate Id on SaveChanges
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<bool> UpdateTaskItemAsync(TaskItem taskItem)
        {
            var existing = await _context.TaskItems.FindAsync(taskItem.Id);
            if (existing == null)
            {
                return false;
            }

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

        public async Task<bool> DeleteTaskItemAsync(int id)
        {
            var existing = await _context.TaskItems.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            _context.TaskItems.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
