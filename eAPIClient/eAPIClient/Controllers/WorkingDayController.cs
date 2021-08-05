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
    public class WorkingDayController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly AppService app;
        public WorkingDayController(ApplicationDbContext _db,AppService _app)
        {
            db = _db;
            app = _app;
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
            try
            {
                DocumentNumberModel _doc = new DocumentNumberModel();
                if (u.id == Guid.Empty)
                {
                    var data = db.WorkingDays.Where(r => r.business_branch_id == u.business_branch_id && r.outlet_id == u.outlet_id && r.is_closed == false);
                    if (data.Any())
                    {
                        return Ok(data.FirstOrDefault());
                    }
                    _doc = app.GetDocument("WorkingDayNum", u.outlet_id.ToString());
                    u.working_day_number = _doc.id > 0 ? app.GetDocumentFormat(_doc) : "NONE";
                    db.WorkingDays.Add(u);
                }
                else
                {
                    db.WorkingDays.Update(u);
                }
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                //Update Document Number
                await app.UpdateDocument(_doc);
                return Ok(u);
            }catch(Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });
            }
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<WorkingDayModel>> DeleteRecord(int id) //Delete
        {
            try
            {
                var u = await db.WorkingDays.FindAsync(id);
                u.is_deleted = !u.is_deleted;
                db.WorkingDays.Update(u);
                await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(u);
            }
            catch(Exception _ex)
            {
                return BadRequest(new BadRequestModel() { message = _ex.Message });
            }
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
