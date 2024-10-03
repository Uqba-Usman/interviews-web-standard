using api.Data;
using api.Models;
using api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task = api.Models.Entities.Task;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<TasksController> logger;

        // Constructor to initialize DbContext and Logger
        public TasksController(ApplicationDbContext dbContext, ILogger<TasksController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        // GET: api/tasks - Retrieve all tasks with their associated tags
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            logger.LogInformation(
                "Fetching all tasks: {RequestMethod} {RequestPath} at {DateTimeUtc}",
                HttpContext.Request.Method,
                HttpContext.Request.Path,
                DateTime.UtcNow
            );

            // Query tasks and include related tags
            var allTasks = dbContext.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Select(task => new TaskWithTagsDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Tags = task.TaskTags.Select(tt => new TagDto
                    {
                        Id = tt.Tag.Id,
                        Name = tt.Tag.Name
                    }).ToList()
                })
                .ToList();
            
            logger.LogInformation("Successfully retrieved {TaskCount} tasks", allTasks.Count);

            return Ok(allTasks);
        }

        // GET: api/tasks/{id} - Retrieve a specific task by its ID
        [HttpGet("{id:guid}")]
        public IActionResult GetTaskById(Guid id)
        {
            logger.LogInformation("Fetching task with ID: {TaskId}", id);

            // Find the task and include its related tags
            var task = dbContext.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                logger.LogWarning("Task with ID: {TaskId} not found", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Task not found",
                    Detail = $"No task found with ID: {id}"
                });
            }

            logger.LogInformation("Successfully retrieved task with ID: {TaskId}", id);
            return Ok(task);
        }

        // POST: api/tasks - Add a new task
        [HttpPost]
        public IActionResult AddTask(AddTaskDto addTaskDto)
        {
            logger.LogInformation("Creating a new task: {TaskName}", addTaskDto.Name);

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid model state for task creation.");
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid request",
                    Detail = "Task data is not valid"
                });
            }

            // Create the task entity
            var taskEntity = new Task
            {
                Name = addTaskDto.Name,
                Description = addTaskDto.Description
            };

            // Handle tags if provided
            var tags = new List<Tag>();
            if (addTaskDto.TagIds != null && addTaskDto.TagIds.Any())
            {
                // Fetch tags from the database
                tags = dbContext.Tags.Where(tag => addTaskDto.TagIds.Contains(tag.Id)).ToList();

                // If not all tags were found, return a BadRequest
                if (tags.Count != addTaskDto.TagIds.Count)
                {
                    logger.LogWarning("One or more tags not found for task creation.");
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Invalid request",
                        Detail = "One or more tags do not exist."
                    });
                }
            }

            // Add the task-tag relationships
            foreach (var tag in tags)
            {
                taskEntity.TaskTags.Add(new TaskTag
                {
                    TaskId = taskEntity.Id,
                    TagId = tag.Id,
                    AddedOn = DateTime.UtcNow
                });
            }

            // Save the task to the database
            dbContext.Tasks.Add(taskEntity);
            dbContext.SaveChanges();

            logger.LogInformation("Task created successfully with ID: {TaskId}", taskEntity.Id);
            return Ok(taskEntity);
        }

        // PUT: api/tasks/{id} - Update an existing task
        [HttpPut("{id:guid}")]
        public IActionResult UpdateTask(Guid id, UpdateTaskDto updateTaskDto)
        {
            logger.LogInformation("Updating task with ID: {TaskId}", id);

            // Find the task to update
            var task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                logger.LogWarning("Task with ID: {TaskId} not found for update", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Task not found",
                    Detail = $"No task found with ID: {id}"
                });
            }

            // Update the task's properties
            task.Name = updateTaskDto.Name;
            task.Description = updateTaskDto.Description;
            dbContext.SaveChanges();

            logger.LogInformation("Task updated successfully with ID: {TaskId}", id);
            return Ok(task);
        }

        // DELETE: api/tasks/{id} - Delete a task by its ID
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteTask(Guid id)
        {
            logger.LogInformation("Deleting task with ID: {TaskId}", id);

            // Find the task to delete
            var task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                logger.LogWarning("Task with ID: {TaskId} not found for deletion", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Task not found",
                    Detail = $"No task found with ID: {id}"
                });
            }

            // Delete the task and save changes
            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();

            logger.LogInformation("Task deleted successfully with ID: {TaskId}", id);
            return Ok();
        }

        // POST: api/tasks/{id}/tags - Add tags to an existing task
        [HttpPost("{id:guid}/tags")]
        public IActionResult AddTagsToTask(Guid id, [FromBody] AddTagsToTaskDto addTagsToTaskDto)
        {
            logger.LogInformation("Adding tags to task with ID: {TaskId}", id);

            // Find the task and include existing tags
            var task = dbContext.Tasks.Include(t => t.TaskTags).FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                logger.LogWarning("Task with ID: {TaskId} not found for tag addition", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Task not found",
                    Detail = $"No task found with ID: {id}"
                });
            }

            // Fetch the tags to be added
            var tags = dbContext.Tags.Where(tag => addTagsToTaskDto.TagIds.Contains(tag.Id)).ToList();
            if (tags.Count != addTagsToTaskDto.TagIds.Count)
            {
                logger.LogWarning("One or more tags not found for task with ID: {TaskId}", id);
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid request",
                    Detail = "One or more tags do not exist"
                });
            }

            // Add new tags to the task, if not already associated
            foreach (var tag in tags)
            {
                if (!task.TaskTags.Any(tt => tt.TagId == tag.Id))
                {
                    task.TaskTags.Add(new TaskTag
                    {
                        TaskId = task.Id,
                        TagId = tag.Id,
                        AddedOn = DateTime.UtcNow
                    });
                }
            }

            // Save the updated task
            dbContext.SaveChanges();
            logger.LogInformation("Tags added successfully to task with ID: {TaskId}", id);

            return Ok(task);
        }
    }
}
