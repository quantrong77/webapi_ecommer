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
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly myDBContext _context;

        public PurchaseOrdersController(myDBContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetPurchaseOrders()
        {
            var purchaseOrders = await _context.PurchaseOrders
            .Join(_context.Vendors, po => po.VendorId, v => v.Id, (po, v) => new PurchaseOrderDto
            {
               Id = po.Id,
               VendorId = po.VendorId,
               VendorName = v.VendorName,
               CreatedDate = po.CreatedDate,
               TotalAmount = po.TotalAmount,
               PurchaseOrderItems = _context.PurchaseOrderItems
                .Where(item => item.PurchaseOrderId == po.Id)
                .Select(item => new PurchaseOrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                })
                .ToList()
            })

            .ToListAsync();
            purchaseOrders.ForEach(po => po.PurchaseOrderItemCount = po.PurchaseOrderItems.Count);
            return purchaseOrders;
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetPurchaseOrders([FromQuery] int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .Join(_context.Vendors, po => po.VendorId, v => v.Id, (po, v) => new PurchaseOrderDto
                {
                    Id = po.Id,
                    VendorId = po.VendorId,
                    VendorName = v.VendorName,
                    CreatedDate = po.CreatedDate,
                    TotalAmount = po.TotalAmount,
                    PurchaseOrderItems = _context.PurchaseOrderItems
                        .Where(item => item.PurchaseOrderId == po.Id)
                        .Select(item => new PurchaseOrderItemDto
                        {
                            Id = item.Id,
                            ProductId=item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        })
                        .ToList()

                })
                .FirstOrDefaultAsync(po => po.Id == id);
                

            if (purchaseOrder == null)
            {
                return NotFound();
            }
            purchaseOrder.PurchaseOrderItemCount = purchaseOrder.PurchaseOrderItems.Count;
            return purchaseOrder;
        }

        // PUT: api/PurchaseOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PutPurchaseOrder(int id, [FromQuery] PurchaseOrderRequestDto purchaseOrderDto)
        {
            if (id != purchaseOrderDto.Id)
            {
                return BadRequest();
            }

            // Tiến hành cập nhật hoặc tạo mới Purchase Order dựa trên dữ liệu trong purchaseOrderDto

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(id))
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

        // POST: api/PurchaseOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder([FromQuery] PurchaseOrderRequestDto purchaseOrderDto)
        {
            var purchaseOrder = new PurchaseOrder
            {
                VendorId = purchaseOrderDto.VendorId,
                VendorName = purchaseOrderDto.VendorName,
                CreatedDate = purchaseOrderDto.CreatedDate,
                TotalAmount = purchaseOrderDto.TotalAmount,
                PurchaseOrderItems = (ICollection<PurchaseOrderItem>)purchaseOrderDto.PurchaseOrderItems
            };

            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrder", new { id = purchaseOrder.Id }, purchaseOrder);
        }


        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }
    }
}
