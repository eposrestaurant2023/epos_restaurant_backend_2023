using eAPI;
using eAPI.Controllers;
using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eOpticalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public HistoryController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        public async Task<ActionResult<HistoryModel>> Get(string keyword = "")
        {
            var data = await Task.Factory.StartNew(() =>
            db.Histories.Where(r => (
            (r.module ?? "") +
            (r.document_number ?? "") +
            (r.description ?? "")
            ).ToLower().Trim().Contains((keyword ?? "").ToLower().Trim())).AsQueryable());
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);

        }

        [HttpPost("save")]
        public async Task<ActionResult<HistoryModel>> Save([FromBody] HistoryModel h)
        {
            if (h.id == 0)
            {
                db.Histories.Add(h);
            }
            else
            {
                db.Histories.Update(h);
            }



            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
           
            return h;
        }

        [HttpGet("find")]
        [EnableQuery(MaxExpansionDepth = 0)]
        public SingleResult<HistoryModel> Get([FromODataUri] int key)
        {
            var c = db.Histories.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<HistoryModel>> DeleteRecord(int id) //Delete
        {
            var c = await db.Histories.FindAsync(id);
            c.is_deleted = !c.is_deleted;
            db.Histories.Update(c);
            await db.SaveChangesAsync();
            return Ok(c);
        }
    }
}
