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
    public class DiscountCodeController : ODataController
    {

        private readonly ApplicationDbContext db;
        public DiscountCodeController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<DiscountCodeModel> Get()
        {
           
                return db.DiscountCodes;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] DiscountCodeModel u)
        {
           
            
            
            if (u.id == 0)
            {
                db.DiscountCodes.Add(u);
                }
            else
            {
                db.DiscountCodes.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<DiscountCodeModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.DiscountCodes.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.DiscountCodes.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<DiscountCodeModel> Get([FromODataUri] int key)
        {
            var s = db.DiscountCodes.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
