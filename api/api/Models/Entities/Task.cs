namespace api.Models.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
