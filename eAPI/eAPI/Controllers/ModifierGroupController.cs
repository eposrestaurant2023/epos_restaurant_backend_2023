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
    public class ModifierGroupController : ODataController
    {
        private readonly ApplicationDbContext db;
        public ModifierGroupController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        public IQueryable<ModifierGroupModel> Get(string keyword = "")
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return db.ModifierGroups.Where(r =>
                (
                (r.modifier_group_name_en ?? "") +
                (r.modifier_group_name_kh ?? "") 
                ).ToLower().Trim().Contains(keyword.ToLower().Trim()));
            }
            else
            {
                return db.ModifierGroups;
            }
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8)]
        [Route("getsingle")]
        public async Task<SingleResult<ModifierGroupModel>> Get([FromODataUri] int key)
        {
            return await Task.Factory.StartNew(() => SingleResult.Create<ModifierGroupModel>(db.ModifierGroups.Where(r => r.id == key).AsQueryable()));
        }


        [HttpPost("save")]
        public async Task<ActionResult<ModifierGroupModel>> Save([FromBody] ModifierGroupModel u)
        {
            foreach (var pm in u.modifier_group_items)
            {
                pm.children.Where(r => r.id > 0).ToList().ForEach(r => r.modifier = null);
            }

            if (u.id == 0)
            {
                u.modifier_group_product_categories.ForEach(r => r.modifier_group = null);
                u.modifier_group_product_categories.ForEach(r => r.modifer_group_id = 0);
                u.modifier_group_product_categories.ForEach(r => r.id = 0);
                u.modifier_group_product_categories.ForEach(r => r.product_category = null);

                u.modifier_group_items.ForEach(r=>r.id = 0);
                u.modifier_group_items.ForEach(r => r.modifier_group = null);
                u.modifier_group_items.ForEach(r => r.modifier_group_id = 0);
                u.modifier_group_items.ForEach(r => r.modifier = null);
                db.ModifierGroups.Add(u);
            }
            else
            {      
                db.ModifierGroups.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            db.Database.ExecuteSqlRaw($"exec sp_update_product_modifer {u.id},0" );

            db.Database.ExecuteSqlRaw("exec sp_clear_deleted_record");
            return Ok(db.ModifierGroups.Find(u.id));
        }

        [HttpPost]
        [Route("status/{id}")]
        public async Task<ActionResult<ModifierGroupModel>> Status(int id) //Delete
        {
            var u = await db.ModifierGroups.FindAsync(id);
            u.status = !u.status;
            db.ModifierGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost]
        [Route("clone/{id}")]
        public ActionResult<ModifierGroupModel> Clone(int id) //clone
        {
            var u = db.ModifierGroups.Where(r=>r.id == id)
                .Include(r=>r.modifier_group_items.Where(r=>r.is_deleted == false)).ThenInclude(r=>r.modifier)
                .Include(r=>r.modifier_group_items.Where(r=>r.is_deleted == false)).ThenInclude(r=>r.children).ThenInclude(r=>r.modifier)
                .Include(r=>r.modifier_group_product_categories.Where(r=>r.is_deleted == false)).ThenInclude(r=>r.product_category).ToList();
            if (u.Any())
            {
                ModifierGroupModel m = u.FirstOrDefault();
                m.id = 0;
                m.created_date = DateTime.Now;
                m.modifier_group_items.ForEach(r => { r.modifier_group_id = 0; r.id = 0; r.children.ForEach(x => x.id = 0); });
                m.modifier_group_product_categories.ForEach(r => r.modifer_group_id = 0);
                return Ok(m);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult<ModifierGroupModel>> DeleteRecord(int id) //Delete
        {
            var u = await db.ModifierGroups.FindAsync(id);
            u.is_deleted = !u.is_deleted;
            db.ModifierGroups.Update(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }
    }
}
