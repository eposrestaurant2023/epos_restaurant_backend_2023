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
    public class StationController : ODataController
    {

        private readonly ApplicationDbContext db;
        public StationController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<StationModel> Get()
        {
           
                return db.Stations;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] StationModel u)
        {
            
            if (u.id == Guid.Empty)
            {

                db.Stations.Add(u);
            }
            else
            {
                
                db.Stations.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

        [HttpPost("Update")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Update(Guid id, bool is_already_config)
        {
            try
            {
                var s = db.Stations.Find(id);
                s.is_already_config = is_already_config;  
                db.Stations.Update(s);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(s);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost("save/multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<StationModel> stations)
        {
            db.Stations.UpdateRange(stations);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok();
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<StationModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Stations.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Stations.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
