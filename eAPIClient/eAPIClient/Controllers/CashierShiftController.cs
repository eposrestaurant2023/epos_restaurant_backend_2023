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
    public class CashierShiftController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public CashierShiftController(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
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
            try
            {
                u.is_synced = false;
                DocumentNumberModel _doc = new DocumentNumberModel();
                if (u.id == Guid.Empty)
                {
                    var data = db.CashierShifts.Where(r => r.working_day_id == u.working_day_id && r.is_closed == false);
                    if (data.Any())
                    {
                        return Ok(data.FirstOrDefault());
                    }

                    _doc = app.GetDocument("CashierShiftNum", u.cash_drawer_id.ToString());
                    u.cashier_shift_number = _doc.id > 0 ? app.GetDocumentFormat(_doc) : "NONE";
                    db.CashierShifts.Add(u);
                }
                else
                {
                    db.CashierShifts.Update(u);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                db.Database.ExecuteSqlRaw($"exec sp_update_cashier_shift_information '{u.id}'");

                //Update Document
                await app.UpdateDocument(_doc);
                app.sendSyncRequest();
                return Ok(u);
            }
            catch (Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });        
            }
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<CashierShiftModel>> DeleteRecord(int id) //Delete
        {
            try
            {
                var u = await db.CashierShifts.FindAsync(id);
                u.is_deleted = !u.is_deleted;
                u.is_synced = false;  
                db.CashierShifts.Update(u);
                await db.SaveChangesAsync();
                return Ok(u);
            }catch(Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });
            }
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
