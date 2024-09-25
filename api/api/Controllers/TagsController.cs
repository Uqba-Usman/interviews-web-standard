using api.Data;
using api.Models;
using api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TagsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            var allTags = dbContext.Tags.ToList();
            return Ok(allTags);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTagById(Guid id)
        {
            var tag = dbContext.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public IActionResult AddTag(AddTagDto addTagDto)
        {
            var tagEntity = new Tag()
            {
                Name = addTagDto.Name
            };

            dbContext.Tags.Add(tagEntity);
            dbContext.SaveChanges();

            return Ok(tagEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTag(Guid id, UpdateTagDto updateTagDto)
        {
            var tag = dbContext.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            tag.Name = updateTagDto.Name;

            dbContext.SaveChanges();

            return Ok(tag);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTag(Guid id)
        {
            var tag = dbContext.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            dbContext.Tags.Remove(tag);
            dbContext.SaveChanges();
            return Ok();
        }

    }
}
