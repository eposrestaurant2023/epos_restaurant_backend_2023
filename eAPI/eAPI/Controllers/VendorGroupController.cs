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
    public class VendorGroupController : ODataController
    {

        private readonly ApplicationDbContext db;
        public VendorGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<VendorGroupModel> Get()
        {
           
                return db.VendorGroups;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] VendorGroupModel u)
        {
           
            
            
            if (u.id == 0)
            {
                db.VendorGroups.Add(u);
                }
            else
            {
                db.VendorGroups.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<VendorGroupModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.VendorGroups.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.VendorGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<VendorGroupModel> Get([FromODataUri] int key)
        {
            var s = db.VendorGroups.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
