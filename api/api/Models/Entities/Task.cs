namespace api.Models.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public string? Description { get; set; }

        // Navigation property to the join table
        public List<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
