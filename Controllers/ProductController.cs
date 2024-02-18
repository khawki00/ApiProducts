using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                RegistrationDate = DateTime.Now,
                Active = true
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product is null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }
            var productEdit = await _context.Products.FindAsync(id);
            if(productEdit is null)
            {
                return NotFound();
            }
            productEdit.Name = product.Name;
            productEdit.Description = product.Description;
            productEdit.Price = product.Price;
            productEdit.Active = product.Active;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productDelete = await _context.Products.FindAsync(id);
            if(productDelete is null)
            {
                return NotFound();
            }
             _context.Products.Remove(productDelete);
             await _context.SaveChangesAsync();
             return Ok();
        }

        
    }
}