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
    public class TableController : ODataController
    {

        private readonly ApplicationDbContext db;
        public TableController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [AllowAnonymous]
        public IQueryable<TableModel> Get()
        {
           
                return db.Tables;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] TableModel u)
        {
            
            if (u.id == 0)
            {

                db.Tables.Add(u);
            }
            else
            {
                
                db.Tables.Update(u);
            }
            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }

        [HttpPost("Update")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Update(int id, bool is_already_config)
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


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<TableModel>> DeleteRecord(Guid id) //Delete
        {
            var u = await db.Tables.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.Tables.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
