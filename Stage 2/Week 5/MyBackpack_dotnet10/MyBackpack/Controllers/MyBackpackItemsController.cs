using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackpack.Models;

namespace MyBackpack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyBackpackItemsController : ControllerBase
    {
        private readonly MyBackpackContext _context;

        public MyBackpackItemsController(MyBackpackContext context)
        {
            _context = context;
        }

        // GET: api/MyBackpackItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyBackpackItem>>> GetMyBackpackItems()
        {
            return await _context.MyBackpackItems.ToListAsync();
        }

        // GET: api/MyBackpackItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MyBackpackItem>> GetMyBackpackItem(long id)
        {
            var myBackpackItem = await _context.MyBackpackItems.FindAsync(id);

            if (myBackpackItem == null)
            {
                return NotFound();
            }

            return myBackpackItem;
        }

        // PUT: api/MyBackpackItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyBackpackItem(long id, MyBackpackItem myBackpackItem)
        {
            if (id != myBackpackItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(myBackpackItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyBackpackItemExists(id))
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

        // POST: api/MyBackpackItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MyBackpackItem>> PostMyBackpackItem(MyBackpackItem myBackpackItem)
        {
            _context.MyBackpackItems.Add(myBackpackItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMyBackpackItem", new { id = myBackpackItem.Id }, myBackpackItem);
            return CreatedAtAction(nameof(GetMyBackpackItem), new { id = myBackpackItem.Id }, myBackpackItem);
        }

        // DELETE: api/MyBackpackItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyBackpackItem(long id)
        {
            var myBackpackItem = await _context.MyBackpackItems.FindAsync(id);
            if (myBackpackItem == null)
            {
                return NotFound();
            }

            _context.MyBackpackItems.Remove(myBackpackItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MyBackpackItemExists(long id)
        {
            return _context.MyBackpackItems.Any(e => e.Id == id);
        }
    }
}
