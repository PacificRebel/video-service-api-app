using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoServiceApiApp.Models;

namespace VideoServiceApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoItemsController : ControllerBase
    {
        private readonly VideoServiceContext _context;

        public VideoItemsController(VideoServiceContext context)
        {
            _context = context;
        }

        // GET: api/VideoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoItemDTO>>> GetVideoItems()
        {
            return await _context.VideoItems
                 .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/VideoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoItemDTO>> GetVideoItem(long id)
        {
            var videoItemDTO = await _context.VideoItems
                .Where(x => x.Id == id)
                .Select(x => ItemToDTO(x))
                .SingleAsync();

            if (videoItemDTO == null)
            {
                return NotFound();
            }

            return videoItemDTO;
        }

        // PUT: api/VideoItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoItem(long id, VideoItemDTO videoItemDTO)
        {
            if (id != videoItemDTO.Id)
            {
                return BadRequest();
            }

            var videoItem = await _context.VideoItems.FindAsync(id);
            if (videoItem == null)
            {
                return NotFound();
            }

            videoItem.Name = videoItemDTO.Name;
            videoItem.IsComplete = videoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!VideoItemExists(id))
            {
                    return NotFound();
            }
              
            return NoContent();
        }

        // POST: api/VideoItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<VideoItem>> CreateVideoItem(VideoItemDTO videoItemDTO)
        {
            var videoItem = new VideoItem
            {
                IsComplete = videoItemDTO.IsComplete,
                Name = videoItemDTO.Name
            };

            _context.VideoItems.Add(videoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetVideoItem),
                new { id = videoItem.Id },
                ItemToDTO(videoItem));
        }

        // DELETE: api/VideoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoItem(long id)
        {
            var videoItem = await _context.VideoItems.FindAsync(id);

            if (videoItem == null)
            {
                return NotFound();
            }

            _context.VideoItems.Remove(videoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoItemExists(long id) =>
            _context.VideoItems.Any(e => e.Id == id);

        private static VideoItemDTO ItemToDTO(VideoItem videoItem) =>
            new VideoItemDTO
            {
                Id = videoItem.Id,
                Name = videoItem.Name,
                IsComplete = videoItem.IsComplete
            };
    }
}
