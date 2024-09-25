namespace api.Models
{
    public class UpdateTaskDto
    {
        public required string Name { get; set; }

        public string? Description { get; set; }
    }
}
