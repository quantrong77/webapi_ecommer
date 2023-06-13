using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_ecommer.Data;
using WebAPI_ecommer.Models;

namespace WebAPI_ecommer.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsController : ControllerBase
    {
        private readonly myDBContext _context;

        public ProductsController(myDBContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products
                 .Select(p => new Product
                 {
                     Id = p.Id,
                     Sku =p.Sku,
                     Name = p.Name,
                     Description = p.Description ?? string.Empty,
                     UnitPrice = p.UnitPrice,
                     ImageUrl = p.ImageUrl ?? string.Empty,
                     Active = p.Active,
                     UnitsInStock = p.UnitsInStock,
                     DateCreated = p.DateCreated,
                     LastUpdated = p.LastUpdated,// == null ? p.LastUpdated : DateTime.MinValue,
                     CategoryId = p.CategoryId,
                     Category = p.Category
                 })
                .ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Getproduct([FromQuery] int id)
        {
            var product = await _context.Products
                  .Where(p=> p.Id==id)
                  .Select(p => new Product
                  {
                      Id = p.Id,
                      Sku = p.Sku,
                      Name = p.Name,
                      Description = p.Description ?? string.Empty,
                      UnitPrice = p.UnitPrice,
                      ImageUrl = p.ImageUrl ?? string.Empty,
                      Active = p.Active,
                      UnitsInStock = p.UnitsInStock,
                      DateCreated = p.DateCreated,
                      LastUpdated = p.LastUpdated,
                      CategoryId = p.CategoryId,
                      Category = p.Category
                  })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
          
        }

        //GET: api/product/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetCategoryStats()
        {
            var categoryStats = await _context.ProductCategories
                .Select(category => new
                {
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName,
                    ProductCount = category.CategoryName.Count()
                })
                .ToListAsync();

            return Ok(categoryStats);
        }

      
        // PUT: api/products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Putproduct(int id, [FromQuery] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(id))
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

        // POST: api/products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
      
        public async Task<ActionResult<Product>> Postproduct([FromQuery] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getproduct", new { Id = product.Id, Sku= product.Sku,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                ImageUrl = product.ImageUrl,
                Active=product.Active,
                UnitsInStock=product.UnitsInStock,
                DateCreated=product.DateCreated,
                LastUpdated=product.LastUpdated,
                CategoryId=product.CategoryId}, product);
        }

        // DELETE: api/products/5
      
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Deleteproduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool productExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
