namespace api.Models
{
    public class AddTaskDto
    {

        public required string Name { get; set; }

        public string? Description { get; set; }

        public List<Guid> TagIds { get; set; } = new List<Guid>();

    }
}
