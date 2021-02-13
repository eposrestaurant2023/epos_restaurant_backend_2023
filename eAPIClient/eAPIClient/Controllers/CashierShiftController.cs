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
    public class CashierShiftController : ODataController
    {

        private readonly ApplicationDbContext db;
        public CashierShiftController(ApplicationDbContext _db)
        {
            db = _db;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CashierShiftModel> Get()
        {
           
                return db.CashierShifts;
           
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CashierShiftModel u)
        {
         
            if (u.id == Guid.Empty)
            {
                var data = db.CashierShifts.Where(r => r.working_day_id == u.working_day_id && r.is_closed == false);
                if (data.Any())
                {
                    return Ok(data.FirstOrDefault());
                }
                db.CashierShifts.Add(u);
                }
            else
            {
                db.CashierShifts.Update(u);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);


        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CashierShiftModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.CashierShifts.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            
            db.CashierShifts.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CashierShiftModel> Get([FromODataUri] Guid key)
        {
            var s = db.CashierShifts.Where(r => r.id == key).AsQueryable();

            return SingleResult.Create(s);
        }
    }

}
