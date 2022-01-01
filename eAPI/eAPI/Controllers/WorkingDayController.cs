using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using eAPI.Services;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;   
using NETCore.Encrypt;
using System.Text.Json;
using eAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace eAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkingDayController : ODataController
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public WorkingDayController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub; 
        }        

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]   
        public IQueryable<WorkingDayModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
              return (from r in db.WorkingDays
                           where 
                                 EF.Functions.Like(
                                     (
                                        (r.closed_by ?? " ") + 
                                        (r.close_note ?? " ") +
                                        (r.open_note ?? " ") +
                                        (r.working_day_number ?? " ")
                                     ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                           select r);

            }
            else
            {
                return db.WorkingDays.AsQueryable();
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<WorkingDayModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<WorkingDayModel>(db.WorkingDays.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] WorkingDayModel model)
        {
            try
            {
                var workingDayCheck = db.WorkingDays.Where(r => r.id == model.id).AsNoTracking();
                if (workingDayCheck.Count() > 0)
                {
                    db.WorkingDays.Update(model);
                }
                else
                {
                    db.WorkingDays.Add(model);
                }

                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var _ex = ex;
                return BadRequest();
            }
        }

    }
}