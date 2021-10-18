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
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post(Transaction model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.DateOfTransaction = DateTime.Now;

            Product product = await _context.Products.FindAsync(model.ProductSKU);

            if(product == null)
            {
                return BadRequest("Product SKU not found");
            }

            if(!product.IsInStock)
            {
                return BadRequest("Product is out of stock");
            }

            if(product.NumberInStock < model.ItemCount)
            {
                return BadRequest("Not enough in stock");
            }

            product.NumberInStock -= model.ItemCount;

            _context.Transactions.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }
    }
}
