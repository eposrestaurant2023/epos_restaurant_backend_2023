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
        public async Task<ActionResult<string>> Save([FromBody] ModifierGroupModel u)
        {
            db.Database.ExecuteSqlRaw($"delete tbl_modifier where modifier_group_id = {u.id}");
            db.Database.ExecuteSqlRaw($"delete tbl_modifier_group_item where modifier_group_id = {u.id}");
            db.Database.ExecuteSqlRaw($"delete tbl_modifier_group_product_category where modifer_group_id = {u.id}");
            db.Modifiers.AddRange(u.modifiers);
            db.ModifierGroupItems.AddRange(u.modifier_group_items);
            db.ModifierGroupProductCategories.AddRange(u.modifier_group_product_categories);
            if (u.id == 0)
            {
                db.ModifierGroups.Add(u);
            }
            else
            {             
                
                db.ModifierGroups.Update(u);
            }            
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
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
        public async Task<ActionResult<ModifierGroupModel>> Clone(int id) //Delete
        {
            var u = await db.ModifierGroups.FindAsync(id);
            u.id = 0;
            u.created_date = DateTime.Now;
            return Ok(u);
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
