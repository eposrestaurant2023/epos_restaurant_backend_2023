using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Authorize]
    [Route("api/[Controller]")]
    public class CustomerController:ODataController
    {
        private readonly ApplicationDbContext db;
        public CustomerController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string keyword="")
        {
            var c = db.Customers.ToList();
            return Ok(c);
        }

        public async Task<ActionResult<CustomerModel>> Save([FromBody] CustomerModel c,[FromQuery] bool allow_duplicate_name)
        {
            if (allow_duplicate_name)
            {
                if (c.id == 0)
                {
                    db.Customers.Add(c);
                }
                else
                {
                    db.Customers.Update(c);
                }
            }
            else
            {

            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(c);
        }


    }
}
