namespace api.Models
{
    public class TaskWithTagsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
