using System.ComponentModel.DataAnnotations;

namespace PersonalTaskManager.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        // Navigation collection
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}