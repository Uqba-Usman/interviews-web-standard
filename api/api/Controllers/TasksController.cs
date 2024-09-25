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
            var allTasks = dbContext.Tasks.ToList();
            return Ok(allTasks);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTaskById(Guid id)
        {
            var task = dbContext.Tasks.Find(id);
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
    }
}
