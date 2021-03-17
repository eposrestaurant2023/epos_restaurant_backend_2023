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
    public class StockLocationController : ODataController
    {

        private readonly ApplicationDbContext db;
        public StockLocationController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<StockLocationModel> Get()
        {

            return db.StockLocations;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] StockLocationModel u)
        {
           
            
            
            if (u.id == Guid.Empty)
            {
                db.StockLocations.Add(u);
                }
            else
            {
                db.StockLocations.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<StockLocationModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.StockLocations.FindAsync(id);
            //u.is_deleted = !u.is_deleted;
            
            db.StockLocations.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<StockLocationModel> Get([FromODataUri] Guid key)
        {
            var s = db.StockLocations.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
