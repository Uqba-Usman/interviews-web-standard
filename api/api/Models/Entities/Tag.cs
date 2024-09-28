using System.ComponentModel.DataAnnotations;

namespace api.Models.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tag Name is required")]
        [StringLength(50, ErrorMessage = "Tag Name can't be longer than 50 characters")]
        public string Name { get; set; } = string.Empty;

        // Navigation property to the join table
        public List<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
