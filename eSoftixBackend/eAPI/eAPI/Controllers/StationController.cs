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
            try
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
                db.Database.ExecuteSqlRaw($"exec sp_update_station_information '{u.id}'");
                return Ok(u);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return StatusCode(503);


        }

        [HttpPost("Update")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Update(Guid id, bool is_already_config)
        {
            try
            {
                var s = db.Stations.Find(id);
                db.Stations.Update(s);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                db.Database.ExecuteSqlRaw($"exec sp_update_station_information '{id}'");
                return Ok(s);
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<StationModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Stations.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.Stations.Update(u);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            db.Database.ExecuteSqlRaw($"exec sp_update_station_information '{id}'");
            return Ok(u);
        }

        [HttpPost]
        [Route("FullLicense")]
        public async Task<ActionResult<StationModel>> FullLicense([FromBody]StationModel station) //Delete
        {
            var s = await db.Stations.FindAsync(station.id);
            var user = await db.Users.FindAsync(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            s.is_full_license = !s.is_full_license;
            s.full_license_by = user.full_name;
            s.full_license_date = station.full_license_date;
            db.Stations.Update(s);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(s);
        }
    }

}
