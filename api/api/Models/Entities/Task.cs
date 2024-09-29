using System.ComponentModel.DataAnnotations;

namespace api.Models.Entities
{
    public class Task
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Task Name is required")]
        [StringLength(100, ErrorMessage = "Task Name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(50000, ErrorMessage = "Description can't be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        // Navigation property to the join table
        public List<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
