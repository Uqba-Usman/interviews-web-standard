namespace api.Models.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
