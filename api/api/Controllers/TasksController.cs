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
        public TasksController(ApplicationDbContext dbContext, ILogger<TasksController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            try
            {
                logger.LogInformation("Fetching all tasks");

                var allTasks = dbContext.Tasks
                                .Include(t => t.TaskTags)
                                    .ThenInclude(tt => tt.Tag)
                                .ToList();
                return Ok(allTasks);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while getting the tasks: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the tasks.");
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTaskById(Guid id)
        {
            try
            {
                logger.LogInformation("Fetching task with id: {TaskId}", id);

                var task = dbContext.Tasks
                            .Include(t => t.TaskTags)
                                .ThenInclude(tt => tt.Tag)
                            .FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    logger.LogWarning("Task with id: {TaskId} was not found", id);
                    return NotFound();
                }
                return Ok(task);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while getting a single task: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting a single task.");
            }
        }

        [HttpPost]
        public IActionResult AddTask(AddTaskDto addTaskDto)
        {
            try
            {
                logger.LogInformation("Creating a new task: {TaskName}", addTaskDto.Name);

                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid model state for task creation.");
                    return BadRequest(ModelState);  // Returns validation error messages
                }

                var taskEntity = new Task()
                {
                    Name = addTaskDto.Name,
                    Description = addTaskDto.Description
                };

                dbContext.Tasks.Add(taskEntity);
                dbContext.SaveChanges();

                logger.LogInformation("Task created successfully with id: {TaskId}", taskEntity.Id);
                return Ok(taskEntity);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while creating the task: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the task.");
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTask(Guid id, UpdateTaskDto updateTaskDto)
        {
            try
            {
                logger.LogInformation("Updating a task: {TaskName}", updateTaskDto.Name);

                var task = dbContext.Tasks.Find(id);
                if (task == null)
                {
                    logger.LogWarning("Task with id: {TaskId} was not found", id);
                    return NotFound();
                }
                task.Name = updateTaskDto.Name;
                task.Description = updateTaskDto.Description;

                dbContext.SaveChanges();

                logger.LogInformation("Task updated successfully with id: {TaskId}", task.Id);
                return Ok(task);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while updating the task: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the task.");
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
                logger.LogInformation("Deleting a task with Id: {TaskId}", id);
                var task = dbContext.Tasks.Find(id);
                if (task == null)
                {
                    logger.LogWarning("Task with id: {TaskId} was not found", id);
                    return NotFound();
                }

                dbContext.Tasks.Remove(task);
                dbContext.SaveChanges();

                logger.LogInformation("Task deleted successfully with id: {TaskName}", task.Name);

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while deleting the task: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the task.");
            }
        }

        [HttpPost]
        [Route("{id:guid}/tags")]
        public IActionResult AddTagsToTask(Guid id, [FromBody] AddTagsToTaskDto addTagsToTaskDto)
        {
            try
            {
                // Find the task
                var task = dbContext.Tasks.Include(t => t.TaskTags).FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    return NotFound();
                }

                // Find the tags by their IDs
                var tags = dbContext.Tags.Where(tag => addTagsToTaskDto.TagIds.Contains(tag.Id)).ToList();
                if (tags.Count != addTagsToTaskDto.TagIds.Count)
                {
                    return BadRequest("One or more tags do not exist.");
                }

                // Associate tags with the task via TaskTag
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

                dbContext.SaveChanges();

                return Ok(task);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while associating tag with a task: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while associating tag with a task.");
            }
        }
    }
}
