using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eAPIClient;
using eAPIClient.Models;
using eAPIClient.Services;
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
    public class CashDrawerAmountController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly ISyncService sync;
        public CashDrawerAmountController(ApplicationDbContext _db, ISyncService _sync)
        {
            db = _db;
            sync = _sync;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)] 
        public IQueryable<CashDrawerAmountModel> Get()
        {  
                return db.CashDrawerAmounts;     
        }

        
        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] CashDrawerAmountModel u)
        {
            u.is_synced = false;
            try
            {
                if (u.id == Guid.Empty)
                {
                  
                    db.CashDrawerAmounts.Add(u);
                }
                else
                {
                    db.CashDrawerAmounts.Update(u);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                sync.sendSyncRequest();
                return Ok(u);
            }
            catch (Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });        
            }
        }

        [HttpPost("multiple")]
        public async Task<ActionResult<string>> SaveMultiple([FromBody] List<CashDrawerAmountModel> u)
        {
            try
            {
                db.CashDrawerAmounts.UpdateRange(u);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(u);
            }
            catch (Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });
            }
        }


        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<CashDrawerAmountModel>> DeleteRecord([FromBody] CashDrawerAmountModel u) //Delete
        {
            try
            {
                var c = await db.CashDrawerAmounts.FindAsync(u.id);
                c.is_deleted = true;
                c.deleted_date = u.deleted_date;
                c.deleted_by = u.deleted_by;
                c.deleted_note = u.deleted_note;
                db.CashDrawerAmounts.Update(c);
                await db.SaveChangesAsync();
                return Ok(u);
            }catch(Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });
            }
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public SingleResult<CashDrawerAmountModel> Get([FromODataUri] Guid key)
        {
            var s = db.CashDrawerAmounts.Where(r => r.id == key).AsQueryable();     
            return SingleResult.Create(s);
        }
    }

}
