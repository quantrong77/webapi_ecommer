using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_ecommer.Data;
using WebAPI_ecommer.Dto;
using WebAPI_ecommer.Models;

namespace WebAPI_ecommer.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    

    public class CategoriesController : ControllerBase
    {
        private readonly myDBContext _context;

        public CategoriesController(myDBContext context)
        {
            _context = context;
        }

        // GET: api/product_category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductCategories()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        // GET: api/product_category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Getproduct_category(int id)
        {
            var product_category = await _context.ProductCategories.FindAsync(id);

            if (product_category == null)
            {
                return NotFound();
            }

            return product_category;
        }


        //GET:api/product_category/stats
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryStats()
        {
            var categoryCount = await _context.ProductCategories.CountAsync();
                     
            return Ok(categoryCount);
        }

        // PUT: api/product_category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPut("{id}")]
        [Authorize]
       
        public async Task<IActionResult> Putproduct_category(int id, [FromQuery] Category product_category)
        {

         
            if (id != product_category.Id)
            {
                return BadRequest();
            }

            _context.Entry(product_category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!product_categoryExists(id))
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

        // POST: api/product_category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        [Authorize]
        
        public async Task<ActionResult<Category>> Postproduct_category([FromQuery] Category product_category)
        {
            _context.ProductCategories.Add(product_category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getproduct_category", new {id = product_category.Id }, product_category);
        }

        // DELETE: api/product_category/5
        
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Deleteproduct_category( int id)
        {
            var product_category = await _context.ProductCategories.FindAsync(id);
            if (product_category == null)
            {
                return NotFound();
            }

            _context.ProductCategories.Remove(product_category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool product_categoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }
    }
}
