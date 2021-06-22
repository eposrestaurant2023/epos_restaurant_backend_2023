using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]"),Authorize]
    public class CustomerGroupController:ODataController
    {
        private readonly ApplicationDbContext db;
        public CustomerGroupController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [EnableQuery,HttpGet]
        public IActionResult Get()
        {
            return Ok(db.CustomerGroups);
        }
        [Route("save")]
        public async Task<ActionResult> Save([FromBody] CustomerGroupModel group)
        {
            if (group.id == 0)
            {
                List<CustomerGroupModel> exsisting_data = db.CustomerGroups.Where(r => r.customer_group_name_kh.Trim().ToLower() == group.customer_group_name_kh.Trim().ToLower() && r.is_deleted == false).ToList();
                if (exsisting_data.Any())
                {
                    return Conflict($"Customer group {group.customer_group_name_kh} already exist!");
                }
                exsisting_data = db.CustomerGroups.Where(r => r.customer_group_name_en.Trim().ToLower() == group.customer_group_name_kh.Trim().ToLower() && r.id != group.id).ToList();
                if (exsisting_data.Any())
                {
                    return Conflict($"Customer group {group.customer_group_name_en} already exist!");
                }
                db.CustomerGroups.Add(group);
            }
            else
            {
                List<CustomerGroupModel> exsisting_data = db.CustomerGroups.Where(r => r.customer_group_name_kh.Trim().ToLower() == group.customer_group_name_kh.Trim().ToLower() && r.id != group.id && r.is_deleted == false).ToList();
                if (exsisting_data.Any())
                {
                    return Conflict($"Customer group {group.customer_group_name_kh} already exist!");
                }
                exsisting_data = db.CustomerGroups.Where(r => r.customer_group_name_en.Trim().ToLower() == group.customer_group_name_kh.Trim().ToLower() && r.id != group.id).ToList();
                if (exsisting_data.Any())
                {
                    return Conflict($"Customer group {group.customer_group_name_en} already exist!");
                }
                db.CustomerGroups.Update(group);
            }

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(group);
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0)]
        [Route("find")]

        public SingleResult<CustomerGroupModel> Get([FromODataUri] int key)
        {
            var c = db.CustomerGroups.Where(r => r.id == key).AsQueryable();
            return SingleResult.Create(c);
        }

        [HttpPost("delete/{id}")]
        public async Task<ActionResult<CustomerGroupModel>> Delete(int id)
        {
            var g = await db.CustomerGroups.FindAsync(id);
            g.is_deleted = !g.is_deleted;
            db.CustomerGroups.Update(g);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(g);
        }


        [HttpPost("status/{id}")]
        public async Task<ActionResult<CustomerGroupModel>> ChangeStatus(int id)
        {
            var g = await db.CustomerGroups.FindAsync(id);
            g.status = !g.status;
            db.Update(g);
            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(g);
        }


    }
}
