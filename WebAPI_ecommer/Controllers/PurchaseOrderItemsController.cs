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
    public class PurchaseOrderItemsController : ControllerBase
    {
        private readonly myDBContext _context;

        public PurchaseOrderItemsController(myDBContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrderItemDto>>> GetPurchaseOrderItems()
        {
            var purchaseOrderItems = await _context.PurchaseOrderItems
                .Include(item => item.PurchaseOrder)
                .Include(item => item.Product)
                .Select(item => new PurchaseOrderItemDto
                {
                     Id = item.Id,
                     //PurchaseOrderId = item.PurchaseOrderId,
                     ProductId = item.ProductId,
                     //ProductName = item.Product.Name,
                     Quantity = item.Quantity,
                     UnitPrice = item.UnitPrice
                })
                .ToListAsync();

                return purchaseOrderItems;
        }

        // GET: api/PurchaseOrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderItemDto>> GetPurchaseOrderItem([FromQuery] int id)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems
             .Include(item => item.PurchaseOrder)
             .Include(item => item.Product)
             .Select(item => new PurchaseOrderItemDto
                {
                     Id = item.Id,
                     //PurchaseOrderId = item.PurchaseOrderId,
                     ProductId = item.ProductId,
                     //ProductName = item.Product.Name,
                     Quantity = item.Quantity,
                     UnitPrice = item.UnitPrice
                })
            .FirstOrDefaultAsync(item => item.Id == id);

            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            return purchaseOrderItem;
        }

        // PUT: api/PurchaseOrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
     
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPurchaseOrderItem(int id, [FromQuery] PurchaseOrderItemDto purchaseOrderItemDto)
        {
            if (id != purchaseOrderItemDto.Id)
            {
                return BadRequest();
            }

            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            // Cập nhật các thuộc tính của purchaseOrderItem dựa trên purchaseOrderItemDto
            purchaseOrderItem.ProductId = purchaseOrderItemDto.ProductId;
            //purchaseOrderItem.ProductName= purchaseOrderItemDto.ProductName;
            purchaseOrderItem.Quantity = purchaseOrderItemDto.Quantity;
            purchaseOrderItem.UnitPrice = purchaseOrderItemDto.UnitPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderItemExists(id))
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
        // POST: api/PurchaseOrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
 
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PurchaseOrderItem>> PostPurchaseOrderItem([FromQuery] PurchaseOrderItemDto purchaseOrderItemDto)
        {
            // Tạo một đối tượng PurchaseOrderItem từ purchaseOrderItemDto
            var purchaseOrderItem = new PurchaseOrderItem
            {
                //ProductId = purchaseOrderItemDto.ProductId,
                //ProductName = purchaseOrderItemDto.ProductName,
                Quantity = purchaseOrderItemDto.Quantity,
                UnitPrice = purchaseOrderItemDto.UnitPrice
            };

            _context.PurchaseOrderItems.Add(purchaseOrderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrderItem", new { id = purchaseOrderItem.Id }, purchaseOrderItem);
        }

        // DELETE: api/PurchaseOrderItems/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePurchaseOrderItem(int id)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            _context.PurchaseOrderItems.Remove(purchaseOrderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseOrderItemExists(int id)
        {
            return _context.PurchaseOrderItems.Any(e => e.Id == id);
        }
    }
}
