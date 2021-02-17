using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class UnitController : ODataController
    {
        private readonly ApplicationDbContext db;
        public UnitController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<UnitModel> Get()
        {            
            return db.Units;
        }
        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] UnitModel u)
        {            
            if (u.id == 0)
            {
                db.Units.Add(u);
            }
            else
            { 
                db.Units.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<UnitModel> Get([FromODataUri] int key)
        {
            var s = db.Units.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(s);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<UnitModel>> UpdateStatus(int id)
        {
            var d = await db.Units.FindAsync(id);
            d.status = !d.status;
            db.Units.Update(d);
            await db.SaveChangesAsync();
            return Ok(d);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<UnitModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Units.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.Units.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
