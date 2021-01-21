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
            
            if (u.id == 0)
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
