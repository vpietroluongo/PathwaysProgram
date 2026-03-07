using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardApi.Models;

namespace FlashCardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardItemsController : ControllerBase
    {
        private readonly FlashCardContext _context;

        public FlashCardItemsController(FlashCardContext context)
        {
            _context = context;
        }

        // GET: api/FlashCardItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlashCardItem>>> GetFlashCardItems()
        {
            return await _context.FlashCardItems.ToListAsync();
        }

        // GET: api/FlashCardItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlashCardItem>> GetFlashCardItem(long id)
        {
            var flashCardItem = await _context.FlashCardItems.FindAsync(id);

            if (flashCardItem == null)
            {
                return NotFound();
            }

            return flashCardItem;
        }

        // PUT: api/FlashCardItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlashCardItem(long id, FlashCardItem flashCardItem)
        {
            if (id != flashCardItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(flashCardItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlashCardItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FlashCardItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlashCardItem>> PostFlashCardItem(FlashCardItem flashCardItem)
        {
            _context.FlashCardItems.Add(flashCardItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetFlashCardItem", new { id = flashCardItem.Id }, flashCardItem);
            return CreatedAtAction(nameof(GetFlashCardItem), new { id = flashCardItem.Id }, flashCardItem);
        }

        // DELETE: api/FlashCardItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlashCardItem(long id)
        {
            var flashCardItem = await _context.FlashCardItems.FindAsync(id);
            if (flashCardItem == null)
            {
                return NotFound();
            }

            _context.FlashCardItems.Remove(flashCardItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlashCardItemExists(long id)
        {
            return _context.FlashCardItems.Any(e => e.Id == id);
        }
    }
}
