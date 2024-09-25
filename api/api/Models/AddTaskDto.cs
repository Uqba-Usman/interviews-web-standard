namespace api.Models
{
    public class AddTaskDto
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

    }
}
