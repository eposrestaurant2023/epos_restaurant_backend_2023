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
            
            if (u.id ==Guid.Empty)
            {

                db.StockLocations.Add(u);
            }
            else
            {
                
                db.StockLocations.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw($"exec sp_update_stock_location_information '{u.id}'");
            return Ok(u);


        }

        [HttpPost("Update")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Update(Guid id, bool is_already_config)
        {
            try
            {
                var s = db.StockLocations.Find(id);
                db.StockLocations.Update(s);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(s);
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<StockLocationModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.StockLocations.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.StockLocations.Update(u);
            await db.SaveChangesAsync();
            db.Database.ExecuteSqlRaw($"exec sp_update_stock_location_information '{u.id}'");
            return Ok(u);
        }
    }

}
