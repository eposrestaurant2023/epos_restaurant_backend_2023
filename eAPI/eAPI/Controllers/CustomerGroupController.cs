using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomerGroupController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public CustomerGroupController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
      
        public IQueryable<CustomerGroupModel> Get(string keyword ="" )
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return db.CustomerGroups.Where(r =>
                (
                (r.customer_group_name_en ?? "") +
                (r.customer_group_name_kh?? "")
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.CustomerGroups;
            }
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
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CustomerGroupModel> Get([FromODataUri] int key)
        {
            var s = db.CustomerGroups.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
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

        [HttpPost]
        [Route("clone/{id}")]
        public async Task<ActionResult<CustomerGroupModel>> CloneRecord(int id) //Delete
        {
            var u = await db.CustomerGroups.FindAsync(id);
            u.id = 0;
            u.created_date = DateTime.Now;
            db.CustomerGroups.Add(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
