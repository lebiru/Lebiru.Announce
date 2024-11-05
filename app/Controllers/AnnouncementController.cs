using System.Collections.Generic;
using Lebiru.Announce.Models;
using Lebiru.Announce.Services;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lebiru.Announce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly AnnouncementService _service;

        public AnnouncementController(AnnouncementService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Announcement> GetAll() => _service.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Announcement> Get(int id)
        {
            var announcement = _service.Get(id);
            return announcement == null ? NotFound() : Ok(announcement);
        }

        // [Authorize]
        [HttpPost("Create")]
        public IActionResult Create(Announcement announcement)
        {
            _service.Add(announcement);
            return CreatedAtAction(nameof(Get), new { id = announcement.Id }, announcement);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Announcement announcement)
        {
            if (id != announcement.Id) return BadRequest();

            _service.Update(announcement);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}/render")]
        public IActionResult RenderMarkdown(int id)
        {
            var announcement = _service.Get(id);
            if (announcement == null) return NotFound();

            var html = Markdown.ToHtml(announcement.Content);
            return Content(html, "text/html");
        }
    }
}
