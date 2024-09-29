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
        private readonly ILogger<TasksController> logger;

        public TagsController(ApplicationDbContext dbContext, ILogger<TasksController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            try
            {
                logger.LogInformation("Fetching all tags");
                var allTags = dbContext.Tags.ToList();
                return Ok(allTags);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while getting all tags: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the tags.");
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTagById(Guid id)
        {
            try
            {

                logger.LogInformation("Fetching tag with id: {TagId}", id);
                var tag = dbContext.Tags.Find(id);
                if (tag == null)
                {
                    logger.LogWarning("Tag with id: {TagId} was not found", id);
                    return NotFound();
                }
                return Ok(tag);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while getting tag by Id: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the single tag.");
            }
        }

        [HttpPost]
        public IActionResult AddTag(AddTagDto addTagDto)
        {
            try
            {
                logger.LogInformation("Creating a new tag: {TagName}", addTagDto.Name);

                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid model state for tag creation.");
                    return BadRequest(ModelState);  // Returns validation error messages
                }

                var tagEntity = new Tag()
                {
                    Name = addTagDto.Name
                };

                dbContext.Tags.Add(tagEntity);
                dbContext.SaveChanges();

                logger.LogInformation("Tag created successfully with id: {TagId}", tagEntity.Id);
                return Ok(tagEntity);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while adding tag: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the tag.");
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTag(Guid id, UpdateTagDto updateTagDto)
        {
            try 
            { 
                logger.LogInformation("Updating a tag: {TagName}", updateTagDto.Name);

                var tag = dbContext.Tags.Find(id);
                if (tag == null)
                {
                    logger.LogWarning("Tag with id: {TagId} was not found", id);

                    return NotFound();
                }
                tag.Name = updateTagDto.Name;

                dbContext.SaveChanges();

                logger.LogInformation("Tag updated successfully with id: {TagId}", tag.Id);

                return Ok(tag);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while updating the tag: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the tag.");
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTag(Guid id)
        {
            try
            {
                logger.LogInformation("Deleting a tag with Id: {TagId}", id);

                var tag = dbContext.Tags.Find(id);
                if (tag == null)
                {
                    logger.LogWarning("Tag with id: {TagId} was not found", id);

                    return NotFound();
                }

                dbContext.Tags.Remove(tag);
                dbContext.SaveChanges();

                logger.LogInformation("Tag deleted successfully with id: {TagName}", tag.Name);


                return Ok();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while deleting the tag: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the tag.");
            }
        }

    }
}
