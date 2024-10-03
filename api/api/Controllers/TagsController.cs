using api.Data;
using api.Models;
using api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<TagsController> logger;

        // Constructor to initialize the DbContext and Logger
        public TagsController(ApplicationDbContext dbContext, ILogger<TagsController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        // GET: api/tags - Retrieve all tags
        [HttpGet]
        public IActionResult GetAllTags()
        {
            logger.LogInformation(
                "Starting Request {RequestMethod} {RequestPath} at {DateTimeUtc}",
                HttpContext.Request.Method,
                HttpContext.Request.Path,
                DateTime.UtcNow);

            // Retrieve all tags from the database
            var allTags = dbContext.Tags.ToList();
            
            logger.LogInformation("Successfully fetched {TagCount} tags.", allTags.Count);
            return Ok(allTags);
        }

        // GET: api/tags/{id} - Retrieve a specific tag by its ID
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTagById(Guid id)
        {
            logger.LogInformation("Fetching tag with id: {TagId}", id);

            // Find the tag by its ID
            var tag = dbContext.Tags.Find(id);
            if (tag == null)
            {                
                logger.LogWarning("Tag with id: {TagId} was not found", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tag not found",
                    Detail = $"No tag found with ID: {id}"
                });
            }
            
            logger.LogInformation("Successfully fetched tag with id: {TagId}", id);
            return Ok(tag);
        }

        // GET: api/tags/{id}/tasks - Retrieve all tasks associated with a specific tag by its ID
        [HttpGet]
        [Route("{id:guid}/tasks")]
        public IActionResult GetTasksByTagId(Guid id)
        {            
            logger.LogInformation("Fetching tasks associated with tag id: {TagId}", id);

            // Find the tag with its associated tasks
            var tag = dbContext.Tags.Include(t => t.TaskTags)
                                    .ThenInclude(tt => tt.Task)
                                    .FirstOrDefault(t => t.Id == id);

            if (tag == null)
            {
                logger.LogWarning("Tag with id: {TagId} was not found", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tag not found",
                    Detail = $"No tag found with ID: {id}"
                });
            }

            // Prepare the DTO with the tag's tasks
            var tagWithTasksDto = new TagWithTasksDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Tasks = tag.TaskTags.Select(tt => new TaskDto
                {
                    Id = tt.Task.Id,
                    Name = tt.Task.Name,
                    Description = tt.Task.Description
                }).ToList()
            };
            
            logger.LogInformation("Successfully fetched {TaskCount} tasks for tag with id: {TagId}", tagWithTasksDto.Tasks.Count, id);
            return Ok(tagWithTasksDto);
        }

        // POST: api/tags - Create a new tag
        [HttpPost]
        public IActionResult AddTag(AddTagDto addTagDto)
        {
            logger.LogInformation("Creating a new tag: {TagName}", addTagDto.Name);

            // Validate the model
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid model state for tag creation.");
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid request",
                    Detail = "Tag data is not valid"
                });
            }

            // Create a new tag entity
            var tagEntity = new Tag
            {
                Name = addTagDto.Name
            };

            // Add the new tag to the database and save changes
            dbContext.Tags.Add(tagEntity);
            dbContext.SaveChanges();

            logger.LogInformation("Tag created successfully with id: {TagId}", tagEntity.Id);
            return Ok(tagEntity);
        }

        // PUT: api/tags/{id} - Update an existing tag
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTag(Guid id, UpdateTagDto updateTagDto)
        {            
            logger.LogInformation("Updating tag with id: {TagId}, Name: {TagName}", id, updateTagDto.Name);

            // Find the tag by its ID
            var tag = dbContext.Tags.Find(id);
            if (tag == null)
            {                
                logger.LogWarning("Tag with id: {TagId} was not found", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tag not found",
                    Detail = $"No tag found with ID: {id}"
                });
            }

            // Update the tag's name
            tag.Name = updateTagDto.Name;
            dbContext.SaveChanges();
            
            logger.LogInformation("Tag updated successfully with id: {TagId}", tag.Id);
            return Ok(tag);
        }

        // DELETE: api/tags/{id} - Delete a tag by its ID
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTag(Guid id)
        {
            logger.LogInformation("Deleting tag with id: {TagId}", id);

            // Find the tag by its ID
            var tag = dbContext.Tags.Find(id);
            if (tag == null)
            {                
                logger.LogWarning("Tag with id: {TagId} was not found", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tag not found",
                    Detail = $"No tag found with ID: {id}"
                });
            }

            // Remove the tag from the database and save changes
            dbContext.Tags.Remove(tag);
            dbContext.SaveChanges();
            
            logger.LogInformation("Tag deleted successfully with id: {TagId}", id);
            return Ok();
        }
    }
}
