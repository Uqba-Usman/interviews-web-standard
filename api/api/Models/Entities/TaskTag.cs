namespace api.Models.Entities
{
    public class TaskTag
    {

        public Guid TaskId { get; set; }
        public required Task Task { get; set; }

        public Guid TagId { get; set; }
        public required Tag Tag { get; set; }

    }
}
