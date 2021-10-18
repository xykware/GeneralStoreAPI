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
    public class CustomerController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        [Route("api/Customer/Post")]
        public async Task<IHttpActionResult> Post(Customer model)
        {
            if (model == null)
            {
                return BadRequest("No information given");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(model);
            int rows = await _context.SaveChangesAsync();

            return Ok($"Added {rows} customer");
        }

        [HttpGet]
        [Route("api/Customer/GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        [HttpGet]
        [Route("api/Customer/{id}/Get")]
        public async Task<IHttpActionResult> Get(int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPut]
        [Route("api/Customer/{id}/Update")]
        public async Task<IHttpActionResult> Update([FromUri] int id, [FromBody] Customer model)
        {
            if (model == null)
            {
                return BadRequest("No information given");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return BadRequest("Given ID does not exist");
            }

            if (model.FirstName != null)
            {
                customer.FirstName = model.FirstName;
            }

            if (model.LastName != null)
            {
                customer.LastName = model.LastName;
            }

            await _context.SaveChangesAsync();

            return Ok($"{customer.FullName} was updated");
        }

        [HttpDelete]
        [Route("api/Customer/{id}/Delete")]
        public async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return BadRequest("Given ID does not exist");
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return Ok($"{customer.FullName} was deleted");
        }
    }
}
