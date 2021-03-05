using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient;
using eAPIClient.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPIClient.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class WorkingDayController : ODataController
    {

        private readonly ApplicationDbContext db;
        public WorkingDayController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<WorkingDayModel> Get()
        {
           
                return db.WorkingDays;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] WorkingDayModel u)
        {
         
            if (u.id == Guid.Empty)
            {
                var data = db.WorkingDays.Where(r => r.business_branch_id == u.business_branch_id && r.outlet_id == u.outlet_id && r.is_closed == false);
                if (data.Any())
                {
                    return Ok(data.FirstOrDefault());
                }
                db.WorkingDays.Add(u);
                }
            else
            {
                db.WorkingDays.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<WorkingDayModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.WorkingDays.FindAsync(id);
            u.is_deleted = !u.is_deleted;  
            db.WorkingDays.Update(u);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));           
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<WorkingDayModel> Get([FromODataUri] Guid key)
        {
            var s = db.WorkingDays.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
