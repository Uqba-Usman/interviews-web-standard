using api.Data;
using api.Models;
using api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = api.Models.Entities.Task;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TasksController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            //var allTasks = dbContext.Tasks.ToList();
            //return Ok(allTasks);
            var allTasks = dbContext.Tasks
                            .Include(t => t.TaskTags)
                                .ThenInclude(tt => tt.Tag) // Load tags through TaskTags
                            .ToList();
            return Ok(allTasks);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTaskById(Guid id)
        {
            //var task = dbContext.Tasks.Find(id);
            //if (task == null)
            //{
            //    return NotFound();
            //}
            //return Ok(task);
            var task = dbContext.Tasks
                        .Include(t => t.TaskTags)
                            .ThenInclude(tt => tt.Tag)
                        .FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public IActionResult AddTask(AddTaskDto addTaskDto)
        {
            var taskEntity = new Task()
            {
                Name = addTaskDto.Name,
                Description = addTaskDto.Description
            };

            dbContext.Tasks.Add(taskEntity);
            dbContext.SaveChanges();

            return Ok(taskEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTask(Guid id, UpdateTaskDto updateTaskDto)
        {
            var task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            task.Name = updateTaskDto.Name;
            task.Description = updateTaskDto.Description;

            dbContext.SaveChanges();

            return Ok(task);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("{id:guid}/tags")]
        public IActionResult AddTagsToTask(Guid id, [FromBody] AddTagsToTaskDto addTagsToTaskDto)
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
    }
}
