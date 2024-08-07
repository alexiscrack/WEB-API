using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace API.FurnitureStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {              //agregar el context para poder utilizarlo
        private readonly APIFurnitureStoreContext _context;

        public ProductsController(APIFurnitureStoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return BadRequest();

            return Ok(product);
        }

        [HttpGet ("GetByCategory/{producCategoryId}") ]
        public async Task<IEnumerable<Product>> GetByCategory(int producCategoryId)
        {
            return await _context.Products // Filtro la tabla productosL
                              .Where(p => p.ProductCategory  == producCategoryId) //traiga los productos que c
                              .ToListAsync();
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Post", product.Id, product);
        }
        [HttpPut]//Endpoints

        public async Task<IActionResult> Put(Product product)

        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Product product)
        {
            if (product == null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}