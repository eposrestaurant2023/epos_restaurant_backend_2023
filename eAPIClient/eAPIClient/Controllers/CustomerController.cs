using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomerController : ODataController
    {

        private readonly ApplicationDbContext db;
        public CustomerController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CustomerModel> Get(string keyword="")
        {
            if(string.IsNullOrEmpty(keyword))
            {
                return db.Customers;

            }
            else
            {
                return (from r in db.Customers
                        where
                              EF.Functions.Like(
                                  (
                                     (r.customer_code ?? " ") +
                                     (r.customer_name_en ?? " ") +
                                     (r.customer_name_kh ?? " ") 
                                  ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r);

            }



        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CustomerModel u)
        {
         
            if (u.id == Guid.Empty)
            {
                db.Customers.Add(u);
            }
            else
            {
                db.Customers.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CustomerModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Customers.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.Customers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CustomerModel> Get([FromODataUri] Guid key)
        {
            var s = db.Customers.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }
    }

}
