using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackpack.Models;
using MyBackpack.Services;

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

        // GET: api/MyBackpackItems/totalweight
        [HttpGet("totalWeight")]
        //public async Task<ActionResult<Dictionary<string, double>>> GetTotalWeight()
        public async Task<ActionResult<double>> GetTotalWeight()
        {
            List<MyBackpackItem> items = await _context.MyBackpackItems.ToListAsync();
            double pounds = 0;
            double totalWeightInPounds = 0;
            //double baseWeightInPounds = 0;

            //var totalsByUnit = items
            //    .GroupBy(item => item.WeightUnit)
            //    .ToDictionary(
            //        group => group.Key,
            //        group => group.Sum(item => item.Weight)
            //    );
            foreach (MyBackpackItem item in items)
            {
                if (item.WeightUnit != "lb")
                {
                    IConvert toPoundsConverter = new ToPounds();
                    UnitConversionService unitConversionService = new UnitConversionService(toPoundsConverter);
                    pounds = unitConversionService.Convert(item.Weight, item.WeightUnit);
                }
                else
                {
                    pounds = item.Weight;
                }
                totalWeightInPounds += pounds;
                //Console.WriteLine(Math.Round(totalWeightInPounds, 2));
                //if (item.IsConsumable == "Yes" || item.IsWorn == "Yes")
                //    baseWeightInPounds += pounds;
            }

            return Math.Round(totalWeightInPounds, 2);
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
