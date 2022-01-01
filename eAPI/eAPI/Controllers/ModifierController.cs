using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using eAPI.Hubs;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;


namespace eAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ModifierController : ODataController
    {

        private readonly ApplicationDbContext db;
        private readonly IHubContext<ConnectionHub> hub;
        public ModifierController(ApplicationDbContext _db,IHubContext<ConnectionHub> _hub)
        {
            db = _db;hub = _hub;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ModifierModel> Get(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var c = from r in db.Modifiers
                        where EF.Functions.Like((
                            (r.modifier_name ?? "") 
                    ).ToLower().Trim(), $"%{keyword}%".ToLower().Trim())
                        select r;
                return c.AsQueryable();
            }
            else
            {
                return db.Modifiers;
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ModifierModel>> Get([FromODataUri] Guid key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ModifierModel>(db.Modifiers.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ModifierModel u)
        {
            if (u.id == Guid.Empty)
            {
                db.Modifiers.Add(u);
            }
            else
            {
                db.Modifiers.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),hub);
            return Ok(u);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<ModifierModel>> ChangeStatus(string id) //Delete
        {
            var u = await db.Modifiers.FindAsync(Guid.Parse(id));
            u.status = !u.status;
            db.Modifiers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet]
        [Route("clone/{id}")]
        public ActionResult<ModifierModel> Clone(string id) //clone
        {
            var u = db.Modifiers.Find(Guid.Parse(id));
            u.id = Guid.Empty;
            u.is_deleted = false;
            u.status = false;
            u.created_date = DateTime.Now;
            return Ok(u);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ModifierModel>> DeleteRecord(string id) //Delete
        {
            var u = await db.Modifiers.FindAsync(Guid.Parse(id));
            u.is_deleted = !u.is_deleted;            
            db.Modifiers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
