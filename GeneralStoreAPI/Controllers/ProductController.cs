using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        [Route("api/Product/Post")]
        public async Task<IHttpActionResult> Post(Product model)
        {
            if(model == null)
            {
                return BadRequest("No information given");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(model);
            int rows = await _context.SaveChangesAsync();

            return Ok($"Added {rows} product");
        }

        [HttpGet]
        [Route("api/Product/GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpGet]
        [Route("api/Product/{sku}/Get")]
        public async Task<IHttpActionResult> Get(string sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut]
        [Route("api/Product/{sku}/Update")]
        public async Task<IHttpActionResult> Update([FromUri]string sku, [FromBody]Product model)
        {
            if (model == null)
            {
                return BadRequest("No information given");
            }

            Product product = await _context.Products.FindAsync(sku);

            if(product == null)
            {
                return BadRequest("Given SKU does not exist");
            }

            if(model.Name != null)
            {
                product.Name = model.Name;
            }

            if (model.Price != null)
            {
                product.Price = model.Price;
            }

            if (model.Description != null)
            {
                product.Description = model.Description;
            }

            if (model.NumberInStock != null)
            {
                product.NumberInStock = model.NumberInStock;
            }

            await _context.SaveChangesAsync();

            return Ok($"{product.Name} was updated");
        }

        [HttpDelete]
        [Route("api/Product/{sku}/Delete")]
        public async Task<IHttpActionResult> Delete([FromUri] string sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if(product == null)
            {
                return BadRequest("Given SKU does not exist");
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Ok($"{product.Name} was deleted");
        }
    }
}
