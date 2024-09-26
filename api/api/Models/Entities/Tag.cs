namespace api.Models.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        // Navigation property to the join table
        public List<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
