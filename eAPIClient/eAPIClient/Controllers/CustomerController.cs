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
    public class CustomerGroupController : ODataController
    {

        private readonly ApplicationDbContext db;
        public CustomerGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CustomerGroupModel> Get()
        {
           
                return db.CustomerGroups;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CustomerGroupModel u)
        {
         
            if (u.id == 0)
            {
                db.CustomerGroups.Add(u);
            }
            else
            {
                db.CustomerGroups.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CustomerGroupModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.CustomerGroups.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.CustomerGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CustomerGroupModel> Get([FromODataUri] int key)
        {
            var s = db.CustomerGroups.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }
    }

}
