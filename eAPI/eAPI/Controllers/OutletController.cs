using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OutletController : ODataController
    {

        private readonly ApplicationDbContext db;
        public OutletController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<OutletModel> Get()
        {
           
                return db.Outlets;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] OutletModel u)
        {
           
            
            
            if (u.id == Guid.Empty)
            {
                db.Outlets.Add(u);
                }
            else
            {
                db.Outlets.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<OutletModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Outlets.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Outlets.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<OutletModel> Get([FromODataUri] Guid key)
        {
            var s = db.Outlets.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
