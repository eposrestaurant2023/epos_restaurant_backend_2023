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
    public class ModifierController : ODataController
    {

        private readonly ApplicationDbContext db;
        public ModifierController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ModifierModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.Modifiers.Where(r =>
                (
                (r.modifier_name ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.Modifiers;
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ModifierModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ModifierModel>(db.Modifiers.Where(r => r.id == key).AsQueryable()));
        }

        [HttpPost("save")]
        public async Task<ActionResult<string>> Save([FromBody] ModifierModel u)
        {
            if (u.id == 0)
            {
                db.Modifiers.Add(u);
            }
            else
            {
                db.Modifiers.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(u);
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<ModifierModel>> ChangeStatus(int id) //Delete
        {
            var u = await db.Modifiers.FindAsync(id);
            u.status = !u.status;
            db.Modifiers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpGet]
        [Route("clone/{id}")]
        public ActionResult<ModifierModel> Clone(int id) //clone
        {
            var u = db.Modifiers.Find(id);
            u.id = 0;
            u.is_deleted = false;
            u.status = false;
            u.created_date = DateTime.Now;
            return Ok(u);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ModifierModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.Modifiers.FindAsync(id);
            u.is_deleted = !u.is_deleted;            
            db.Modifiers.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }

}
